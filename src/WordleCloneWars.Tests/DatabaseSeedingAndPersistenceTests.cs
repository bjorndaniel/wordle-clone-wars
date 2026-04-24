namespace WordleCloneWars.Tests;

public class DatabaseSeedingAndPersistenceTests
{
    [Fact]
    public async Task EnsureGameInfo_adds_all_game_types_once_and_is_idempotent()
    {
        await using var context = await CreateInMemoryContextAsync();

        await SeedData.EnsureGameInfo(context);
        await SeedData.EnsureGameInfo(context);

        var gameInfos = await context.GameInfos.ToListAsync();

        Assert.Equal(Enum.GetValues<GameType>().Length, gameInfos.Count);
        Assert.Equal(gameInfos.Count, gameInfos.Select(g => g.Type).Distinct().Count());
    }

    [Fact]
    public async Task EnsureGameInfo_sets_utc_offsets_for_started_at()
    {
        await using var context = await CreateInMemoryContextAsync();

        await SeedData.EnsureGameInfo(context);

        var gameInfos = await context.GameInfos.ToListAsync();

        Assert.All(gameInfos, gameInfo => Assert.Equal(TimeSpan.Zero, gameInfo.StartedAt.Offset));
    }

    [Fact]
    public async Task SaveRound_sets_authenticated_user_and_utc_completion_time()
    {
        var (context, factory) = await CreateInMemoryContextWithFactoryAsync();
        await using var _ = context;
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "tester",
            Email = "tester@example.com",
            UserName = "tester@example.com"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var authStateProvider = new Mock<AuthenticationStateProvider>();
        authStateProvider
            .Setup(p => p.GetAuthenticationStateAsync())
            .ReturnsAsync(CreateAuthenticatedState(user.Id));

        var logger = new Mock<ILogger<RoundService>>();
        var service = new RoundService(factory, authStateProvider.Object, logger.Object);

        var inputRound = new Round
        {
            Type = GameType.Wordle,
            Rounds = 6,
            CompletionRound = 3,
            GameRound = 100
        };

        var (success, error) = await service.SaveRound(inputRound);

        Assert.True(success, error);
        Assert.Equal(string.Empty, error);

        var stored = await context.Rounds.SingleAsync();
        Assert.Equal(user.Id, stored.UserId);
        Assert.Equal(TimeSpan.Zero, stored.CompletedDateTime.Offset);
    }

    [Fact]
    public async Task SaveRound_returns_failure_for_unauthenticated_user()
    {
        var (context, factory) = await CreateInMemoryContextWithFactoryAsync();
        await using var _ = context;

        var authStateProvider = new Mock<AuthenticationStateProvider>();
        authStateProvider
            .Setup(p => p.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity())));

        var logger = new Mock<ILogger<RoundService>>();
        var service = new RoundService(factory, authStateProvider.Object, logger.Object);

        var (success, error) = await service.SaveRound(new Round
        {
            Type = GameType.Wordle,
            Rounds = 6,
            CompletionRound = 2,
            GameRound = 123
        });

        Assert.False(success);
        Assert.Equal("Could not save round", error);
        Assert.Empty(context.Rounds);
    }

    private static AuthenticationState CreateAuthenticatedState(string userId)
    {
        var identity = new System.Security.Claims.ClaimsIdentity(
            new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId) },
            authenticationType: "TestAuth");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }

    private static async Task<ApplicationDbContext> CreateInMemoryContextAsync()
    {
        var (context, _) = await CreateInMemoryContextWithFactoryAsync();
        return context;
    }

    private static async Task<(ApplicationDbContext context, IDbContextFactory<ApplicationDbContext> factory)> CreateInMemoryContextWithFactoryAsync()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return (context, new TestDbContextFactory(options));
    }
}
