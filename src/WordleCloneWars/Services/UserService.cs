namespace WordleCloneWars.Services;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ILogger<RoundService> _logger;
    private readonly UserManager<User> _userManager;
    public UserService(ApplicationDbContext dbContext, AuthenticationStateProvider authProvider, ILogger<RoundService> logger, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _authProvider = authProvider;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        if (state?.User?.IsInRole("Administrator") ?? false)
        {
            return await _dbContext
                .Users
                    .OrderBy(_ => _.DisplayName)
                        .ThenBy(_ => _.FirstName)
                        .ThenBy(_ => _.LastName)
                .ToListAsync();
        }

        return new List<User>();
    }

    public async Task ToggleUserLock(string userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            if (user.LockoutEnd.HasValue)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(10);
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public ValueTask<User?> GetByIdAsync(string userId) =>
        _dbContext.Users.FindAsync(userId);

    public async Task<(bool success, string error)> UpdateUserAsync(UpdateUserModel user)
    {
        var dbUser = await _dbContext.Users.FindAsync(user.Id);
        if (_dbContext.Users.Any(_ => _.Id != user.Id && EF.Functions.Like(_.DisplayName, user.DisplayName)))
        {
            return (false, "Displayname already taken");
        }
        dbUser.DisplayName = user.DisplayName;
        if (user.HasUpdatedPassword())
        {
            if (!user.PasswordsEqual())
            {
                return (false, "Password and Repeat password must match");
            }
            var result = await _userManager.ChangePasswordAsync(dbUser, user.CurrentPassword, user.NewPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(_ => _.Description).Aggregate((a, b) => $"{a},{b}"));
            }
        }

        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }
}