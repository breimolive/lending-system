using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Server.Database.Seeds;

namespace Server.Database;

public class DatabaseContextSeed
{
    private readonly DatabaseContext _context;
    private readonly ILogger<DatabaseContextSeed> _logger;

    public DatabaseContextSeed(DatabaseContext context, ILogger<DatabaseContextSeed> logger)
    {
        _context = context;
        _logger = logger;
    }

    public static async Task SeedAsync(IServiceProvider services)
    {
        var policy = RetryPolicy(services);
        await policy.ExecuteAsync(async () =>
        {
            await using var scope = services.CreateAsyncScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseContextSeed>();
            await seeder.TrySeedAsync();
        });
    }

    private async Task TrySeedAsync()
    {
        _logger.LogInformation("Migrating database");
        await _context.Database.MigrateAsync();

        await SeedUser();
        await SeedCategory();
        await SeedEquipment();
        await SeedBorrower();
    }

    private async Task SeedUser()
    {
        if (_context.Users.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding User");

        _context.Users.AddRange(UserEntitySeed.Seeds());
        await _context.SaveChangesAsync();
    }
    
    private async Task SeedCategory()
    {
        if (_context.Categories.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding Category");

        _context.Categories.AddRange(CategoryEntitySeed.Seeds());
        await _context.SaveChangesAsync();
    }
    
    private async Task SeedEquipment()
    {
        if (_context.Equipment.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding Equipment");

        var category = await _context.Categories.FirstOrDefaultAsync();
        if (category == null)
        {
            await SeedCategory();
            return;
        }

        _context.Equipment.AddRange(EquipmentEntitySeed.Seeds(category));
        await _context.SaveChangesAsync();
    }

    private async Task SeedBorrower()
    {
        if (_context.Borrowers.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding Borrower");
        
        _context.Borrowers.Add(BorrowerEntitySeed.Seed());
        await _context.SaveChangesAsync();
    }

    private static AsyncRetryPolicy RetryPolicy(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILogger<DatabaseContextSeed>>();
        return Policy.Handle<Exception>().WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: count => TimeSpan.FromSeconds(Math.Pow(2, count)),
            onRetry: (Exception exception, TimeSpan timeSpan, int retry, Context ctx) =>
            {
                logger.LogWarning(
                    exception,
                    "Database migration or seeding failed on attempt {Retry} of {RetryCount}, sleeping for {TimeSpan}",
                    retry,
                    3,
                    timeSpan
                );
            }
        );
    }
}