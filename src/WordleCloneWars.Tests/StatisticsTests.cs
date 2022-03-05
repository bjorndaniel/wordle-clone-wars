

namespace WordleCloneWars.Tests;

public class StatisticsTests
{
    [Theory]
    [ClassData(typeof(RoundTestData))]
    public void Rounds_with_holes_in_order_gives_correct_streak(
            List<Round> rounds, int expectedCurrentStreak, 
            int expectedRoundsPlayed, int expectedMaxStreak)
    {
        //When
        var result = new Statistics(rounds);
        
        //Then
        Assert.True(expectedCurrentStreak == result.CurrentStreak(), $"Expected current streak {expectedCurrentStreak} but was {result.CurrentStreak()}");
        Assert.True(expectedRoundsPlayed == result.RoundsPlayed, $"Expected {expectedRoundsPlayed} but was {result.RoundsPlayed}");
        Assert.True(expectedMaxStreak == result.MaxStreak(), $"Expected max streak {expectedMaxStreak} but was {result.MaxStreak()}");
    }

    private class RoundTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new List<Round>
                {
                    new()
                    {
                        GameRound = 1,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 2,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 3,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 4,
                        Rounds = 6,
                        CompletionRound = 0,
                    },
                    new()
                    {
                        GameRound = 5,
                        Rounds = 6,
                        CompletionRound = 0,
                    },
                    new()
                    {
                        GameRound = 6,
                        Rounds = 6,
                        CompletionRound = 5,
                    },
                    new()
                    {
                        GameRound = 7,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                },
                2,
                7,
                3
            };
            yield return new object[]
            {
                new List<Round>
                {
                    new()
                    {
                        GameRound = 1,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 3,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 4,
                        Rounds = 6,
                        CompletionRound = 2,
                    },
                    new()
                    {
                        GameRound = 5,
                        Rounds = 6,
                        CompletionRound = 0,
                    },
                    new()
                    {
                        GameRound = 7,
                        Rounds = 6,
                        CompletionRound = 6,
                    },
                    new()
                    {
                        GameRound = 8,
                        Rounds = 6,
                        CompletionRound = 5,
                    },
                },
                2,
                6,
                2
            };
            yield return new object[]
            {
                new List<Round>
                {
                },
                0,
                0,
                0
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}