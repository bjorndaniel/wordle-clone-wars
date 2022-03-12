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
        await EnsureRoles(context);
        await EnsureGameInfo(context);
    }

    private static async Task EnsureRoles(ApplicationDbContext context)
    {
        if (!(await context.Roles.AnyAsync()))
        {
           await context.Roles.AddAsync(new IdentityRole
            {
                Id = "1CD109EB-6307-4A0C-97B1-1B46655EFB49",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
           await context.Roles.AddAsync(new IdentityRole
            {
                Id = "BF0C3522-B203-4127-B420-839C4B14FCF6",
                Name = "User",
                NormalizedName = "USER"
            });
        }
        await context.SaveChangesAsync();
        var admin = await context.Users.FindAsync("45ffd81c-04b3-4952-9f93-6b1bfdcdac76");
        if (admin != null && !context.UserRoles.Any
            (_ => _.RoleId == "1CD109EB-6307-4A0C-97B1-1B46655EFB49" &&
                  _.UserId == "45ffd81c-04b3-4952-9f93-6b1bfdcdac76"))
        {
            await context.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                UserId = "45ffd81c-04b3-4952-9f93-6b1bfdcdac76",
                RoleId = "1CD109EB-6307-4A0C-97B1-1B46655EFB49"
            });
            await context.SaveChangesAsync();
        }
    }

    public static async Task EnsureGameInfo(ApplicationDbContext context)
    {
        foreach (var gameType in Enum.GetValues<GameType>())
        {
            var existing = await context.GameInfos.FirstOrDefaultAsync(_ => _.Type == gameType);
            if (existing == null)
            {
                if(DateTime.TryParse((gameType.GetCustomAttribute<StartDateAttribute>()?.StartDate ?? ""), out var startDate))
                {
                    existing = new GameInfo
                    {
                        Type = gameType,
                        StartedAt = new DateTimeOffset(startDate)
                    };
                    await context.GameInfos.AddAsync(existing);    
                }
            }
        }
        await context.SaveChangesAsync();
    }
}