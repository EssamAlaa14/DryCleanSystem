using Domain.Entities;
using Domain.Entities.Base;
using Domain.Entities.Orders;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
        ) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<CleaningType> CleaningTypes { get; set; } = null!;
        public DbSet<Piece> Pieces { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically apply IEntityTypeConfiguration<> configurations
            var configurations = typeof(ApplicationDbContext).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var configurationType in configurations)
            {
                var configurationInstance = Activator.CreateInstance(configurationType);
                if (configurationInstance != null)
                {
                    modelBuilder.ApplyConfiguration((dynamic)configurationInstance);
                }
            }

            // BaseEntity configuration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Set default value for CreatedAt and UpdatedAt
                    modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity.CreatedAt))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity.UpdatedAt))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseEntity.RecordStatus))
                        .IsRequired()
                        .HasDefaultValue(RecordStatus.Enabled);

                    // Dynamically build a lambda expression for the global query filter
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GenerateNonDeletedRecordStatusLambda(entityType));
                }
            }

            modelBuilder.HasSequence<long>("InvoiceNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);

            base.OnModelCreating(modelBuilder);
        }

        private static LambdaExpression GenerateNonDeletedRecordStatusLambda(IMutableEntityType entityType)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.RecordStatus));
            var comparison = Expression.NotEqual(
                property,
                Expression.Constant(RecordStatus.Deleted)
            );

            return Expression.Lambda(comparison, parameter);
        }
    }
}
