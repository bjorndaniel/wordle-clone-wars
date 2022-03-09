namespace WordleCloneWars.Models;

public class GameInfo
{
    public int Id { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public GameType Type { get; set; }
}