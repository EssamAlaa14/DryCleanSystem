using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbInitializer>>();

            try
            {
                // Migrate the database
                logger.LogInformation("Applying database migrations...");
                await context.Database.MigrateAsync();

                // Seed Cleaning Types
                logger.LogInformation("Seeding cleaning types...");
                await SeedCleaningTypesAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
            }
        }

        private static async Task SeedCleaningTypesAsync(ApplicationDbContext context, ILogger logger)
        {
            if (!context.CleaningTypes.Any())
            {
                var cleaningTypes = new List<CleaningType>
                {
                    new("Washing", "غسيل"),
                    new("Ironing", "مكواة"),
                    new("Dyeing", "صبغة")
                };

                await context.CleaningTypes.AddRangeAsync(cleaningTypes);
                await context.SaveChangesAsync();

                logger.LogInformation("Cleaning types seeded successfully.");
            }
            else
            {
                logger.LogInformation("Cleaning types already exist. Skipping seed.");
            }
        }
    }
}
