namespace WordleCloneWars.Helpers;

public class ApplicationUserClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<User> _userManager;

    public ApplicationUserClaimsTransformation(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
        if (identity == null)
        {
            return principal;
        }

        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
        {
            return principal;
        }

        if (!principal.HasClaim(c => c.Type == ClaimTypes.GivenName) && !string.IsNullOrWhiteSpace(user.FirstName))
        {
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
        }

        if (!principal.HasClaim(c => c.Type == ClaimTypes.Surname) && !string.IsNullOrWhiteSpace(user.LastName))
        {
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
        }
        
        if (!principal.HasClaim(c => c.Type == "DisplayName") && !string.IsNullOrWhiteSpace(user.DisplayName))
        {
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
        }
        return new ClaimsPrincipal(identity);
    }
}