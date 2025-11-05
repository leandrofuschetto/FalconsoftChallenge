using Microsoft.EntityFrameworkCore;
using OrderNowChallenge.DAL.Entities;
using System.Reflection;

namespace OrderNowChallenge.DAL
{
    public class OrderNowDbContext : DbContext
    {
        public OrderNowDbContext(DbContextOptions<OrderNowDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ClientEntity> Clients => Set<ClientEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
