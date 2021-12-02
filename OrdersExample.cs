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

        public static async Task Run()
        {
            await using var context = new OrderContext();
            await context.Database.EnsureCreatedAsync();

            await CreateOrdersAsync(context);

            Console.WriteLine("Getting orders...");
            var page = 1;
            const int pageSize = 10;
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
                await Task.Delay(TimeSpan.FromMilliseconds(50));
            } while (orders.Length > 0);
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