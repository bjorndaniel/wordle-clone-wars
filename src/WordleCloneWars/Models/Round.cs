namespace WordleCloneWars.Models;
public class Round
{
    public int Id { get; set; }
    public GameType Type { get; set; }
    public int? CompletionRound { get; set; }
    public int Rounds { get; set; }
    public int GameRound { get; set; }
    // public int UserId { get; set; }
}