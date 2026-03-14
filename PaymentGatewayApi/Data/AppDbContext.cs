using Microsoft.EntityFrameworkCore;
using PaymentGatewayApi.Models;

namespace PaymentGatewayApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Gateway> Gateways { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionProduct> TransactionProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionProduct>()
                .HasKey(tp => new { tp.TransactionId, tp.ProductId });
        }
    }
}