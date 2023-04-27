namespace WordleCloneWars.Models;
public enum GameType
{
    [StartDate(StartDate = "2021-06-19")]
    Wordle,
    [StartDate(StartDate = "2022-01-07")]
    Ordsnille,
    [StartDate(StartDate = "2022-01-05")]
    Ordlig,
    [StartDate(StartDate = "2022-01-19")]
    Nerdle,
    [StartDate(StartDate = "2021-12-31")]
    Ordel
}

public enum HighScoreType
{
    HighestStreakHistorically,
    HighestCurrentStreak,
    DailyTopResult
}

