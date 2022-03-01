namespace WordleCloneWars.Models;
public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public List<Round> Rounds { get; set; } = new();
}