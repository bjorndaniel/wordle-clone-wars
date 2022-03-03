
namespace WordleCloneWars.Models;
public class User : IdentityUser
{
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [MaxLength(50)]
    public string? LastName { get; set; }
    [Required, MaxLength(50)]
    public string DisplayName { get; set; } = null!;
    public List<Round> Rounds { get; set; } = new();
}