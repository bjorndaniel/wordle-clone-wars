namespace WordleCloneWars.Data;

public class SeedData
{
    public static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if(context == null)
        {
            throw new InvalidOperationException("Could not create ApplicationDbContext");
        }
        await context.Database.MigrateAsync();
    }
}