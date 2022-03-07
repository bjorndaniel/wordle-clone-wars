namespace WordleCloneWars.Helpers;

public class RoundHelper
{
    public static Round? GetRound(string s)
    {
        switch (s)
        {
            case string o when o.Contains("Ordsnille"):
                int.TryParse(s.Split("(")[1][..1], out var roundNrOS);
                int.TryParse(s.Split("/")[1][..1], out var nrRoundsOS);
                int.TryParse(s.Split(" ")[1].Replace("nr", ""), out var gameRoundOS);
                return new Round
                {
                    Type = GameType.Ordsnille,
                    CompletionRound = roundNrOS,
                    Rounds = nrRoundsOS,
                    GameRound = gameRoundOS
                };
            case string w when w.Contains("Wordle"):
                int.TryParse(s.Split("/")[0][^1].ToString(), out var wr);
                return new Round
                {
                    Type = GameType.Wordle,
                    CompletionRound = wr,
                    Rounds = int.Parse(s.Split("/")[1][..1]),
                    GameRound = int.Parse(s.Split(" ")[1])
                };
            case string w when w.Contains("nerdle"):
                int.TryParse(s.Split("/")[0][^1].ToString(), out var nr);
                return new Round
                {
                    Type = GameType.Nerdle,
                    CompletionRound = nr,
                    Rounds = int.Parse(s.Split("/")[1][..1]),
                    GameRound = int.Parse(s.Split(" ")[1])
                };
            case string w when w.Contains("ordlig"):
                var cleaned = s
                    .Replace("http://ordlig.se", "")
                    .Replace("https://ordlig.se", "")
                    .Replace("WEEKEND", "")
                    .Trim();
                var points = cleaned.Split(",", StringSplitOptions.RemoveEmptyEntries)[1];
                int.TryParse(points.Split("/", StringSplitOptions.RemoveEmptyEntries)[0][^1].ToString(), out var roundNr);
                int.TryParse(points.Split("/", StringSplitOptions.RemoveEmptyEntries)[1][..1], out var nrRounds);
                int.TryParse(cleaned.Split("nr")[1].Trim().Split(",")[0].Replace(",", ""), out var gameRound);
                return new Round
                {
                    Type = GameType.Ordlig,
                    CompletionRound = roundNr,
                    Rounds = nrRounds,
                    GameRound = gameRound
                };
            default:
                return null;
        }
    }

}