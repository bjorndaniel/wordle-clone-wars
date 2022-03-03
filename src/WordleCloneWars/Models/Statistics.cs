namespace WordleCloneWars.Models;

public class Statistics
{
    private readonly List<Round> _rounds;

    public Statistics(List<Round> rounds)
    {
        _rounds = rounds;
    }

    public int RoundsPlayed => _rounds.Count();

    public double WinPercentage =>
        _rounds.Any() ? ((double)_rounds.Count(_ => _.CompletionRound > 0)/ _rounds.Count()) * 100 : 0;

    public int CurrentStreak()
    {
        var count = 0;
        foreach (var round in _rounds.OrderByDescending(_ => _.GameRound))
        {
            if (round.CompletionRound > 0)
            {
                count++;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    public int MaxStreak()
    {
        var streaks = new List<int>();
        var count = 0;
        foreach (var round in _rounds.OrderByDescending(_ => _.GameRound))
        {
            if (round.CompletionRound > 0)
            {
                count++;
            }
            else
            {
                streaks.Add(count);
                count = 0;
            }
        }
        streaks.Add(count);

        return streaks.Max();
    }
}