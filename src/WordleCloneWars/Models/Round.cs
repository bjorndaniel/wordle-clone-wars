namespace WordleCloneWars.Models;
public class Round
{
    public int Id { get; set; }
    public GameType Type { get; set; }
    public int? CompletionRound { get; set; }
    public int Rounds { get; set; }
    public int GameRound { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTimeOffset CompletedDateTime { get; set; }
}