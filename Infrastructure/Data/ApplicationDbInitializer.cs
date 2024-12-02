using Domain.Entities;
using Domain.Entities.Orders;
using Domain.Enums;
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
                logger.LogInformation("Seeding...");
                await SeedCleaningTypesAsync(context, logger);
                await SeedPiecesAsync(context, logger);
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
                    new("غسيل"),
                    new("مكواة"),
                    new("صبغة")
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

        private static async Task SeedPiecesAsync(ApplicationDbContext context, ILogger logger)
        {
            if (!context.Pieces.Any())
            {
                var pieces = new List<Piece>
                {
                    new("تيشرت نص", ClothingType.ExternalClothing),
                    new("تيشرت كم", ClothingType.ExternalClothing),
                    new("بنطلون جينز", ClothingType.ExternalClothing),
                    new("سويت بانتس", ClothingType.ExternalClothing),
                    new("سويت شيرت", ClothingType.ExternalClothing),
                    new("جاكيت", ClothingType.ExternalClothing),
                    new("قميص نص", ClothingType.ExternalClothing),
                    new("قميص كم", ClothingType.ExternalClothing),
                    new("شرابات", ClothingType.ExternalClothing),
                    new("طقم داخلي", ClothingType.InternalClothing),
                    new("تيشرت نص داخلي", ClothingType.InternalClothing),
                    new("تيشرت كم داخلي", ClothingType.InternalClothing),
                    new("طقم دفاية", ClothingType.InternalClothing),
                    new("سجادة", ClothingType.Carpet),
                    new("مشاية", ClothingType.Rug),
                    new("دواسة", ClothingType.Rug),
                    new("ستارة", ClothingType.Linen),
                    new("طقم سرير", ClothingType.Linen),
                    new("بطانية", ClothingType.Linen),
                    new("لحاف", ClothingType.Linen),
                    new("خدادية", ClothingType.Linen),
                    new("مخدة", ClothingType.Linen),
                };

                await context.Pieces.AddRangeAsync(pieces);
                await context.SaveChangesAsync();

                logger.LogInformation("Pieces seeded successfully.");
            }
            else
            {
                logger.LogInformation("Pieces already exist. Skipping seed.");
            }
        }
    }
}
