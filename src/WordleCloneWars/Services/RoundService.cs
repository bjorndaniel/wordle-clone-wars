namespace WordleCloneWars.Services;

public class RoundService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthenticationStateProvider _authProvider;

    public RoundService(ApplicationDbContext dbContext, AuthenticationStateProvider authProvider)
    {
        _dbContext = dbContext;
        _authProvider = authProvider;
    }

    public async Task<bool> SaveRound(Round currentRound)
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        var user = state.User;
        var id = user?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (user.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(id))
        {
            currentRound.UserId = id;
            await _dbContext.Rounds.AddAsync(currentRound);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}