namespace WordleCloneWars.Models;
public enum GameType
{
    [StartDate(StartDate = "2021-06-19")]
    [GameUrl(Url = "https://www.nytimes.com/games/wordle/index.html")]
    Wordle,
    [StartDate(StartDate = "2022-01-07")]
    [GameUrl(Url = "https://ordsnille.brusman.se/statistik")]
    Ordsnille,
    [StartDate(StartDate = "2022-01-05")]
    [GameUrl(Url = "https://ordlig.se/")]
    Ordlig,
    [StartDate(StartDate = "2022-01-19")]
    [GameUrl(Url = "https://nerdlegame.com/")]
    Nerdle,
    [StartDate(StartDate = "2021-12-31")]
    [GameUrl(Url = "https://ordel.se/")]
    Ordel
}

public enum HighScoreType
{
    HighestStreakHistorically,
    HighestCurrentStreak,
    DailyTopResult
}
