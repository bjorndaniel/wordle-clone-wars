namespace WordleCloneWars.Data;

public class SeedData
{
    public static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (context == null)
        {
            throw new InvalidOperationException("Could not create ApplicationDbContext");
        }

        await context.Database.MigrateAsync();
        await EnsureGameInfo(context);
    }

    public static async Task EnsureGameInfo(ApplicationDbContext context)
    {
        foreach (var gameType in Enum.GetValues<GameType>())
        {
            var existing = await context.GameInfos.FirstOrDefaultAsync(_ => _.Type == gameType);
            if (existing == null)
            {
                var startDate = DateTime.Parse(gameType.GetCustomAttribute<StartDateAttribute>().StartDate);
                existing = new GameInfo
                {
                    Type = gameType,
                    StartedAt = new DateTimeOffset(startDate)
                };
                await context.GameInfos.AddAsync(existing);
            }
        }

        await context.SaveChangesAsync();
    }
}