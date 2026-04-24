namespace WordleCloneWars.Tests;

internal sealed class TestDbContextFactory : IDbContextFactory<ApplicationDbContext>
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public TestDbContextFactory(DbContextOptions<ApplicationDbContext> options) => _options = options;

    public ApplicationDbContext CreateDbContext() => new(_options);
}
