namespace WordleCloneWars.Models;
public class HighScore
{
    public GameType Type { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
    public HighScoreType StreakType { get; set; }
}