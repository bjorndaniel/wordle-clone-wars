namespace WordleCloneWars.Services;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ILogger<RoundService> _logger;
    
    public UserService(ApplicationDbContext dbContext, AuthenticationStateProvider authProvider, ILogger<RoundService> logger)
    {
        _dbContext = dbContext;
        _authProvider = authProvider;
        _logger = logger;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        if (state?.User?.IsInRole("Administrator") ?? false)
        {
            return await _dbContext.Users.OrderBy(_ => _.FirstName).ThenBy(_ => _.LastName).ToListAsync();
        }

        return new List<User>();
    }
}