using Microsoft.EntityFrameworkCore;

namespace console
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Distributor> Distributors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                Program._endpointUri,
                Program._primaryKey,
                "EntertainmentDatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .ToContainer("Orders")
                .HasNoDiscriminator()
                .HasPartitionKey(o => o.PartitionKey)
                .UseETagConcurrency()
                .OwnsOne(o => o.ShippingAddress);

            modelBuilder.Entity<Distributor>()
                .ToContainer("Distributors")
                .UseETagConcurrency()
                .OwnsMany(d => d.ShippingCenters);
        }
    }
}