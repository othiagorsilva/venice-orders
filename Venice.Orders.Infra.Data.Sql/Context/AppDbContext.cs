using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.Persistence.Sql
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}