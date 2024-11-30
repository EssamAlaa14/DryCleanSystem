using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Configure PrimaryPhone as required
            builder.Property(c => c.PrimaryPhone)
                .HasConversion(
                    v => v.Number, // Convert Phone to string for storage
                    v => new Phone(v)) // Convert string back to Phone
                .IsRequired();

            // Configure SecondaryPhone as optional
            builder.Property(c => c.SecondaryPhone)
                .HasConversion(
                    v => v != null ? v.Number : null, // Use ternary operator
                    v => v != null ? new Phone(v) : null) // Convert string back to Phone or null
                .IsRequired(false);

            // Configure Email as optional
            builder.Property(c => c.Email)
                .HasConversion(
                    v => v != null ? v.Value : null, // Use ternary operator
                    v => v != null ? new Email(v) : null) // Convert string back to Email or null
                .IsRequired(false);

            // Configure Address as an owned type
            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(250);

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100);

                address.Property(a => a.AppartmentNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                address.Property(a => a.FloorNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                address.Property(a => a.District)
                    .IsRequired()
                    .HasMaxLength(50);

                address.Property(a => a.BuildingName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            // Optional: Configure table name
            builder.ToTable("Customers");
        }
    }
}
