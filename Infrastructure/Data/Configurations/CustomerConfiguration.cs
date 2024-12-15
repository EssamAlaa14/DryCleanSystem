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

            builder.Property(c => c.PrimaryPhone)
                .HasConversion(
                    v => v.Number,
                    v => new Phone(v))
                .IsRequired();

            builder.Property(c => c.SecondaryPhone)
                .HasConversion(
                    v => v != null ? v.Number : null,
                    v => v != null ? new Phone(v) : null)
                .IsRequired(false);

            builder.Property(c => c.Email)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new Email(v) : null)
                .IsRequired(false);

            builder.HasMany(c => c.Addresses)
                 .WithOne(a => a.Customer)
                 .HasForeignKey(a => a.CustomerId);

            builder.ToTable("Customers");
        }
    }
}
