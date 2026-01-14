using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.Persistence.Sql.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CustomerId).IsRequired();
            builder.Property(p => p.Status);

            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Ignore(p => p.Items);
        }
    }
}