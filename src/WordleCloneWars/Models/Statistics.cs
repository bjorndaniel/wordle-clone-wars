namespace WordleCloneWars.Models;

public class Statistics
{
    private readonly List<Round> _rounds = new();

    private Statistics()
    {
    }

    public Statistics(List<Round> rounds)
    {
        _rounds = rounds;
        Username = rounds.FirstOrDefault()?.User?.DisplayName ?? string.Empty;
    }

    public int RoundsPlayed => _rounds.Count();

    public double WinPercentage =>
        _rounds.Any() ? Math.Round((double)_rounds.Count(_ => _.CompletionRound > 0) / _rounds.Count * 100, MidpointRounding.AwayFromZero) : 0;

    public int CurrentStreak()
    {
        if (!_rounds.Any())
        {
            return 0;
        }

        var count = 1;
        var firstRound = _rounds.OrderByDescending(_ => _.GameRound).First();
        if (firstRound.CompletionRound < 1)
        {
            return 0;
        }
        var previous = firstRound.GameRound;
        foreach (var round in _rounds.OrderByDescending(_ => _.GameRound).Skip(1))
        {
            if (round.CompletionRound > 0 && previous == round.GameRound + 1)
            {
                count++;
            }
            else
            {
                break;
            }

            previous = round.GameRound;
        }
        return count;
    }

    public int MaxStreak()
    {
        if (!_rounds.Any())
        {
            return 0;
        }

        var streaks = new List<int>();
        var count = 1;
        var firstRound = _rounds.OrderByDescending(_ => _.GameRound).First();
        var previous = firstRound;
        foreach (var round in _rounds.OrderByDescending(_ => _.GameRound).Skip(1))
        {
            if(previous.CompletionRound < 1)
            {
                streaks.Add(count);
                count = 1;
            }
            else if (round.CompletionRound > 0 && previous.GameRound == round.GameRound + 1)
            {
                count++;
            }
            else
            {
                streaks.Add(count);
                count = 1;
            }
            previous = round;
        }

        streaks.Add(count);

        return streaks.Max();
    }

    public string Username { get; set; } = string.Empty;
}