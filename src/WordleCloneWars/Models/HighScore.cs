namespace WordleCloneWars.Models;
public class HighScore
{
    public GameType Type { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Rounds { get; set; }
    public HighScoreType HighScoreType { get; set; }
    public string DisplayText => Score > 0 ? $"{Type}: {Score}/{Rounds} by {Username}" : $"{Type} has not been played today";
}