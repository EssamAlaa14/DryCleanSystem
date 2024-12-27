using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.OrderConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerId)
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.OrderType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(o => o.OrderStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(o => o.DeliveryDate)
                .IsRequired(false);

            builder.Property(o => o.DeliveryFees)
                .IsRequired(false);

            builder.Property(o => o.InvoiceNumber)
                .HasDefaultValueSql("NEXT VALUE FOR InvoiceNumberSequence")
                .IsRequired();

            builder.Property(o => o.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasIndex(o => o.InvoiceNumber).IsUnique();
        }
    }
}
