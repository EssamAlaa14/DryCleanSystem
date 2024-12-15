using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppartmentNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.FloorNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(a => a.BuildingName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(a => a.District)
                .WithMany()
                .HasForeignKey(a => a.DistrictId)
                .IsRequired();

            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
