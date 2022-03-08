namespace WordleCloneWars.Services;

public class RoundService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ILogger<RoundService> _logger;

    public RoundService(ApplicationDbContext dbContext, AuthenticationStateProvider authProvider, ILogger<RoundService> logger)
    {
        _dbContext = dbContext;
        _authProvider = authProvider;
        _logger = logger;
    }

    public async Task<(bool success, string error)> SaveRound(Round currentRound)
    {
        if (currentRound == null)
        {
            throw new ArgumentNullException(nameof(currentRound));
        }
        var state = await _authProvider.GetAuthenticationStateAsync();
        var user = state.User;
        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if ((!(user.Identity?.IsAuthenticated ?? false)) || string.IsNullOrWhiteSpace(id))
        {
            return (false, "Could not save round");
        }
        try
        {
            currentRound.UserId = id;
            await _dbContext.Rounds.AddAsync(currentRound);
            await _dbContext.SaveChangesAsync();
            return (true, string.Empty);
        }
        catch (Exception e)
        {
            _dbContext.Rounds.Remove(currentRound);
            _logger.LogError(e.ToString());
            return (false, $"{(e.InnerException != null ? e.InnerException.Message : e.Message)}");
        }
    }
    
    public async Task<List<Round>> GetRounds(GameType type, string userId) =>
        await _dbContext.Rounds.Where(_ => _.Type == type && _.UserId == userId).ToListAsync();

    public async Task<List<User>> GetOpponents(string userId) =>
        await _dbContext.Users.Where(_ => _.Id != userId).ToListAsync();

    public async Task<List<HighScore>> GetHighScoresAsync(GameType selectedType)
    {
        var result  = new List<HighScore>();
        var scores = await _dbContext
            .Rounds
                .Include(_ => _.User)
            .Where(_ => _.Type == selectedType)
            .ToListAsync();
        var grouped = scores
            .GroupBy(_ => _.UserId).Select(_ => new Statistics(_.ToList())).ToList();
        var current = grouped.OrderByDescending(_ => _.CurrentStreak()).FirstOrDefault();
        var historic = grouped.OrderByDescending(_ => _.MaxStreak()).FirstOrDefault();
        if (current != null)
        {
            var streak = new HighScore
            {
                Type = selectedType,
                StreakType = HighScoreType.HighestCurrentStreak,
                Score = current.CurrentStreak(),
                Username = current.Username
            };
            result.Add(streak);
        }

        if (historic != null)
        {
            var streak = new HighScore
            {
                Type = selectedType,
                StreakType = HighScoreType.HighestStreakHistorically,
                Score = historic.MaxStreak(),
                Username = historic.Username
            };
            result.Add(streak);
        }
        return result;
    }
}