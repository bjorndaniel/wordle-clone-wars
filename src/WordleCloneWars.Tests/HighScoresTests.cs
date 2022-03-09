using System.Linq;

namespace WordleCloneWars.Tests;

public class HighScoresTests
{
    private readonly ITestOutputHelper _output;
    private static readonly Fixture _fixture = new();

    public HighScoresTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Can_get_highest_streaks()
    {
        //Given
        var (service, user, user1) = await Given_a_list_of_rounds_and_roundservice();

        //When
        var result = await service.GetHighScoresAsync(GameType.Wordle);

        //Then
        Then_correct_user_should_have_streak(result, user, user1);
    }

    [Fact]
    public async Task Can_get_daily_highscores()
    {
        //Given
        var (service, user, user1) = await Given_a_list_of_daily_rounds_and_roundservice();
        
        //When
        var result = await service.GetDailyHighScoresAsync();
        
        //Then
        Correct_scores_should_be_returned(result, user);
    }

    private void Correct_scores_should_be_returned(List<HighScore> result, User user)
    {
        Assert.Equal(4, result.Count);
        Assert.Contains(result,
            _ => _.HighScoreType == HighScoreType.DailyTopResult &&
                 _.Type == GameType.Nerdle &&
                 _.Score == (int)GameType.Nerdle &&
                 _.DisplayText == $"Nerdle: {(int)GameType.Nerdle}/6 by {user.DisplayName}" &&
                 _.Rounds == 6);
        Assert.Contains(result,
            _ => _.HighScoreType == HighScoreType.DailyTopResult &&
                 _.Type == GameType.Wordle &&
                 _.Score == 0 &&
                 _.DisplayText == "Wordle has not been played today" &&
                 _.Rounds == 6);
    }

    private static void Then_correct_user_should_have_streak(List<HighScore> result, User user, User user1)
    {
        Assert.Contains(result,
            _ => _.Type == GameType.Wordle && _.HighScoreType == HighScoreType.HighestCurrentStreak && _.Score == 5);
        Assert.Contains(result,
            _ => _.Username == user.DisplayName && _.HighScoreType == HighScoreType.HighestCurrentStreak);
        Assert.Contains(result,
            _ => _.Type == GameType.Wordle && _.HighScoreType == HighScoreType.HighestStreakHistorically && _.Score == 12);
        Assert.Contains(result,
            _ => _.Username == user1.DisplayName && _.HighScoreType == HighScoreType.HighestStreakHistorically);
    }

    private async Task<(RoundService service, User user, User user1)> Given_a_list_of_rounds_and_roundservice()
    {
        var (context, service, user, user1) = await CreateUsersAndService();
        await CreateRounds(user, user1, context);
        Assert.Equal(35, context.Rounds.Count());
        return (service, user, user1);
    }
    
    
    private async Task<(RoundService service, User user, User user1)> Given_a_list_of_daily_rounds_and_roundservice()
    {
        var (context, service, user, user1) = await CreateUsersAndService();
        foreach (var gameType in Enum.GetValues<GameType>())
        {
            var startDate = DateTime.Parse(gameType.GetCustomAttribute<StartDateAttribute>().StartDate);
            var round = new Round
            {
                Type = gameType,
                Rounds = 6,
                CompletionRound = (int)gameType,
                UserId = user.Id,
                GameRound = (int)DateTimeOffset.UtcNow.Subtract(startDate).TotalDays
            };
            await context.Rounds.AddAsync(round);
        }

        await context.SaveChangesAsync();
        return (service, user, user1);
    }


    private async Task<(ApplicationDbContext context, RoundService service, User user, User user1)> CreateUsersAndService()
    {
        var authProvider = new Mock<AuthenticationStateProvider>();
        var context = await GetInMemoryContextAsync();
        _fixture.Customize<User>(_ => _.Without(_ => _.Rounds));
        var user = _fixture.Create<User>();
        var user1 = _fixture.Create<User>();
        await context.Users.AddAsync(user);
        await context.Users.AddAsync(user1);
        await context.SaveChangesAsync();
        Assert.Equal(2, context.Users.Count());
        var service = new RoundService(context, authProvider.Object, _output.BuildLoggerFor<RoundService>());
        return (context, service, user, user1);
    }

    private static async Task CreateRounds(User user, User user1, ApplicationDbContext context)
    {
        for (int i = 1; i < 15; i++)
        {
            var round = new Round
            {
                UserId = user.Id,
                Type = GameType.Wordle,
                CompletionRound = i < 5 ? 3 : 0,
                Rounds = 3,
                GameRound = i
            };
            var round1 = new Round
            {
                UserId = user1.Id,
                Type = GameType.Wordle,
                CompletionRound = i < 13 ? 1 : 0,
                Rounds = 3,
                GameRound = i
            };
            await context.Rounds.AddAsync(round1);
            await context.Rounds.AddAsync(round);
        }

        for (int i = 15; i < 20; i++)
        {
            var round = new Round
            {
                UserId = user.Id,
                Type = GameType.Wordle,
                CompletionRound = 2,
                Rounds = 3,
                GameRound = i
            };
            if (i % 2 == 0)
            {
                var round1 = new Round
                {
                    UserId = user1.Id,
                    Type = GameType.Wordle,
                    CompletionRound = 0,
                    Rounds = 3,
                    GameRound = i
                };
                await context.Rounds.AddAsync(round1);
            }

            await context.Rounds.AddAsync(round);
        }

        await context.SaveChangesAsync();
    }

    private async Task<ApplicationDbContext> GetInMemoryContextAsync()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .EnableSensitiveDataLogging();
        var context = new ApplicationDbContext(builder.Options);
        context.Database.EnsureCreated();
        await SeedData.EnsureGameInfo(context);
        Assert.Equal(4, context.GameInfos.Count());
        return context;
    }
}