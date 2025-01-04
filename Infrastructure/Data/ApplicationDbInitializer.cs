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
                await SeedDistrictsAsync(context, logger);
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
                    new("غسيل", 1),
                    new("مكواة", 2),
                    new("صبغة", 3)
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
                    new("تيشرت نص", 0, ClothingType.ExternalClothing),
                    new("تيشرت كم", 0, ClothingType.ExternalClothing),
                    new("بنطلون جينز", 1, ClothingType.ExternalClothing),
                    new("سويت بانتس", 1, ClothingType.ExternalClothing),
                    new("سويت شيرت", 2, ClothingType.ExternalClothing),
                    new("جاكيت", 3, ClothingType.ExternalClothing),
                    new("قميص نص", 4, ClothingType.ExternalClothing),
                    new("قميص كم", 4, ClothingType.ExternalClothing),
                    new("شرابات", 5, ClothingType.ExternalClothing),
                    new("طقم داخلي", 6, ClothingType.InternalClothing),
                    new("تيشرت نص داخلي", 6, ClothingType.InternalClothing),
                    new("تيشرت كم داخلي", 6, ClothingType.InternalClothing),
                    new("طقم دفاية", 7, ClothingType.InternalClothing),
                    new("سجادة", 1, ClothingType.Carpet),
                    new("مشاية", 2, ClothingType.Rug),
                    new("دواسة", 3, ClothingType.Rug),
                    new("بطانية", 0, ClothingType.Linen),
                    new("لحاف", 0, ClothingType.Linen),
                    new("ستارة", 1, ClothingType.Linen),
                    new("طقم سرير", 2, ClothingType.Linen),
                    new("خدادية", 3, ClothingType.Linen),
                    new("مخدة", 4, ClothingType.Linen),
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

        private static async Task SeedDistrictsAsync(ApplicationDbContext context, ILogger logger)
        {
            if (!context.Districts.Any())
            {
                var districts = new List<District>
                {
                    new("السيوف شماعة", 0, 0),
                    new("كارفور السيوف", 0, 1),
                    new("عذبة محسن", 0, 2),
                    new("زقزوق", 0, 3),
                    new("ابوسليمان", 0, 4),
                    new("العوايد", 0, 5),
                    new("المراغي", 0, 6),
                    new("المهاجرين", 0, 7),
                    new("فيكتوريا", 0, 8),
                    new("مصطفى كامل", 0, 9),
                    new("سموحة", 0, 10),
                    new("محرم بك", 0, 11)
                };

                await context.Districts.AddRangeAsync(districts);
                await context.SaveChangesAsync();

                logger.LogInformation("Districts seeded successfully.");
            }
            else
            {
                logger.LogInformation("Districts already exist. Skipping seed.");
            }
        }
    }
}
