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
            _logger.LogError(e.ToString());
            return (false, $"{(e.InnerException != null ? e.InnerException.Message : e.Message)}");
        }
    }
    
    public async Task<List<Round>> GetRounds(GameType type, string userId) =>
        await _dbContext.Rounds.Where(_ => _.Type == type && _.UserId == userId).ToListAsync();

    public async Task<List<User>> GetOpponents(string userId) =>
        await _dbContext.Users.Where(_ => _.Id != userId).ToListAsync();

}