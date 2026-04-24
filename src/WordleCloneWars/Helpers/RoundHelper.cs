namespace WordleCloneWars.Helpers;

public class RoundHelper
{
    public static Round? GetRound(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return null;
        }
        try
        {
            return Parse(s);
        }
        catch
        {
            return null;
        }
    }

    private static Round? Parse(string s)
    {
        switch (s)
        {
            case string o when o.Contains("Ordsnille", StringComparison.OrdinalIgnoreCase):
                int.TryParse(s.Split("(")[1][..1], out var roundNrOS);
                int.TryParse(s.Split("/")[1][..1], out var nrRoundsOS);
                var ordsnilleGameRound = new string(s.Split("(")[0].Where(char.IsDigit).ToArray());
                int.TryParse(ordsnilleGameRound, out var gameRoundOS);
                return new Round
                {
                    Type = GameType.Ordsnille,
                    CompletionRound = roundNrOS,
                    Rounds = nrRoundsOS,
                    GameRound = gameRoundOS
                };
            case string w when w.Contains("Wordle"):
                int.TryParse(s.Split("/")[0][^1].ToString(), out var wr);
                var leftW = s.Split("/")[0];
                var wordleGameRound = new string(leftW[..^1].Where(char.IsDigit).ToArray());
                return new Round
                {
                    Type = GameType.Wordle,
                    CompletionRound = wr,
                    Rounds = int.Parse(s.Split("/")[1][..1]),
                    GameRound = int.Parse(wordleGameRound)
                };
            case string w when w.Contains("nerdle", StringComparison.OrdinalIgnoreCase):
                int.TryParse(s.Split("/")[0][^1].ToString(), out var nr);
                var leftN = s.Split("/")[0];
                var nerdleGameRound = new string(leftN[..^1].Where(char.IsDigit).ToArray());
                return new Round
                {
                    Type = GameType.Nerdle,
                    CompletionRound = nr,
                    Rounds = int.Parse(s.Split("/")[1][..1]),
                    GameRound = int.Parse(nerdleGameRound)
                };
            case string w when w.Contains("ordlig", StringComparison.OrdinalIgnoreCase):
                var cleaned = s
                    .Replace("http://ordlig.se", "")
                    .Replace("https://ordlig.se", "")
                    .Replace("WEEKEND", "")
                    .Trim();
                var points = cleaned.Split(",", StringSplitOptions.RemoveEmptyEntries)[1];
                int.TryParse(points.Split("/", StringSplitOptions.RemoveEmptyEntries)[0][^1].ToString(), out var roundNr);
                int.TryParse(points.Split("/", StringSplitOptions.RemoveEmptyEntries)[1][..1], out var nrRounds);
                var ordligGameRoundStr = new string(cleaned.Split(",")[0].Where(char.IsDigit).ToArray());
                int.TryParse(ordligGameRoundStr, out var gameRound);
                return new Round
                {
                    Type = GameType.Ordlig,
                    CompletionRound = roundNr,
                    Rounds = nrRounds,
                    GameRound = gameRound
                };
            case string o when o.Contains("Ordel"):
                int.TryParse(s.Split(" ")[2].Split("/")[0], out var roundNrOR);
                int.TryParse(s.Split("/")[1][..1], out var nrRoundsOR);
                int.TryParse(s.Split("#")[1].Split(" ")[0], out var gameRoundOR);
                return new Round
                {
                    Type = GameType.Ordel,
                    CompletionRound = roundNrOR,
                    Rounds = nrRoundsOR,
                    GameRound = gameRoundOR
                };
            default:
                return null;
        }
    }

}