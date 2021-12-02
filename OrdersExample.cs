using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;

namespace console
{
    public static class OrdersExample
    {
        private static readonly StreetAddress[] Addresses = new Bogus.Faker<StreetAddress>()
            .Generate(5)
            .ToArray();

        [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
        public static async Task Run()
        {
            Console.WriteLine();
            Console.WriteLine("Getting started with Cosmos:");
            Console.WriteLine();

            await using var context = new OrderContext();
            await context.Database.EnsureCreatedAsync();

            await CreateOrdersAsync(context);

            Console.WriteLine("Getting orders...");
            var page = 1;
            const int pageSize = 40;
            Order[] orders;
            do
            {
                var sw = Stopwatch.StartNew();
                var query = context.Orders
                    .WithPartitionKey(nameof(Order))
                    .OrderBy(o => o.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                orders = await query
                    .ToArrayAsync();
                Console.WriteLine($"Page {page} with {orders.Length} orders in {sw.Elapsed}");
                page++;
                //Console.ReadKey();
            } while (orders.Length > 0);


            //foreach (var order in orders)
            //{
            //    Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions
            //    {
            //        WriteIndented = true
            //    }));
            //}

            //Console.WriteLine($"Retrieved {orders.Length} orders. Elapsed: {sw.Elapsed}");
            Console.ReadKey();

            var sw2 = Stopwatch.StartNew();
            orders = await context.Orders.WithPartitionKey(nameof(Order)).Take(50).ToArrayAsync();

            //foreach (var order in orders)
            //{
            //    Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions
            //    {
            //        WriteIndented = true
            //    }));
            //}

            //Console.WriteLine($"Retrieved {orders.Length} orders. Elapsed: {sw.Elapsed}");


            Console.ReadKey();
        }

        [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
        private static async Task CreateOrdersAsync(OrderContext context)
        {
            var count = context.Orders.Count();
            if (count > 0)
            {
                return;
            }

            Console.WriteLine("Creating orders...");

            var newOrders = new Bogus.Faker<Order>()
                .RuleFor(o => o.Id, faker => Guid.NewGuid().ToString())
                .RuleFor(o => o.TrackingNumber, faker => faker.Random.Int(10000))
                .RuleFor(o => o.ShippingAddress, faker => faker.PickRandom(Addresses))
                .RuleFor(o => o.PartitionKey, faker => nameof(Order))
                .GenerateLazy(500);

            context.AddRange(newOrders);

            Console.WriteLine("Saving orders...");
            await context.SaveChangesAsync();
        }
    }
}