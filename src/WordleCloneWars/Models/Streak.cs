namespace WordleCloneWars.Models;

public class Streak
{
    public GameType Type { get; set; }
    public int Rounds { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayText => string.IsNullOrWhiteSpace(Username) ? $"{Type}: No one has a streak recorded" : $"{Type}: {Username} has a {Rounds} round streak";
}