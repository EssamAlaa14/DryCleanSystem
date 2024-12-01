using Domain.Entities.Orders;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.OrderConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.SingleQuantityPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.Color)
                .HasConversion(
                    ic => ic.ColorName,
                    cn => Color.Custom(cn)
                )
                .IsRequired();

            builder.Property(oi => oi.Comment)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(oi => oi.CleaningTypes)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "OrderItemCleaningTypes", // Join table name
                    b => b.HasOne<CleaningType>().WithMany().HasForeignKey("CleaningTypeId").OnDelete(DeleteBehavior.Cascade),
                    b => b.HasOne<OrderItem>().WithMany().HasForeignKey("OrderItemId").OnDelete(DeleteBehavior.Cascade));

        }
    }
}
