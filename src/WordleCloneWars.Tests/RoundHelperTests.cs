using WordleCloneWars.Models;

namespace WordleCloneWars.Tests;

public class RoundHelperTests
{
    [Theory]
    [InlineData(@"Ordsnille nr49 (5/6)
        ⬜️⬜️⬜️⬜️⬜️
        ⬜️⬜️⬜️🟧🟩
        ⬜️⬜️🟩⬜️🟩
        🟩🟩🟩⬜️⬜️
        🟩🟩🟩🟩🟩", 5, 6, 49)]
    [InlineData(@"Ordsnille nr51 (X/6)
        ⬜️⬜️⬜️🟩🟩
        ⬜️⬜️⬜️🟩🟩
        ⬜️⬜️🟩🟩🟩
        ⬜️⬜️⬜️⬜️⬜️
        🟩⬜️⬜️⬜️🟩
        ⬜️🟧🟧⬜️🟧", 0, 6, 51)]
    public void Can_parse_Ordsnille(string input, int expectedCompletion, int expectedRounds, int expectedGameRound)
    {
        var result = RoundHelper.GetRound(input);
        Assert.Equal(expectedCompletion, result?.CompletionRound);
        Assert.Equal(expectedRounds, result?.Rounds);
        Assert.Equal(expectedGameRound, result?.GameRound);
        Assert.Equal(GameType.Ordsnille, result?.Type);
    }

    [Theory]
    [InlineData(@"Wordle 251 6/6

        ⬛⬛⬛🟨⬛
        ⬛⬛⬛⬛🟩
        ⬛⬛🟨⬛🟩
        ⬛⬛🟨⬛⬛
        ⬛🟩🟩🟩🟩
        🟩🟩🟩🟩🟩", 6, 6, 251)]
    [InlineData(@"Wordle 253 X/6

        🟨⬛⬛⬛⬛
        🟨⬛⬛⬛⬛
        🟨⬛⬛⬛⬛
        🟨⬛⬛⬛⬛
        🟨⬛⬛⬛⬛
        🟨⬛⬛⬛⬛", 0, 6, 253)]
    public void Can_parse_Wordle(string input, int expectedCompletion, int expectedRounds, int expectedGameRound)
    {
        var result = RoundHelper.GetRound(input);
        Assert.Equal(expectedCompletion, result?.CompletionRound);
        Assert.Equal(expectedRounds, result?.Rounds);
        Assert.Equal(expectedGameRound, result?.GameRound);
        Assert.Equal(GameType.Wordle, result?.Type);
    }

    [Theory]
    [InlineData(@"nerdlegame 35 4/6

        ⬛🟩⬛⬛⬛🟩🟩🟪
        🟩🟩🟪🟩⬛🟩🟩⬛
        🟩🟩⬛🟩🟪🟩🟩🟪
        🟩🟩🟩🟩🟩🟩🟩🟩", 4, 6, 35)]
    public void Can_parse_Nerdle(string input, int expectedCompletion, int expectedRounds, int expectedGameRound)
    {
        var result = RoundHelper.GetRound(input);
        Assert.Equal(expectedCompletion, result?.CompletionRound);
        Assert.Equal(expectedRounds, result?.Rounds);
        Assert.Equal(expectedGameRound, result?.GameRound);
        Assert.Equal(GameType.Nerdle, result?.Type);
    }


    [Theory]
    [InlineData(@"http://ordlig.se http://ordlig.se WEEKEND nr 53, 4/6
        ⬜⬜🟩⬜🟩⬜  
        ⬜⬜🟩⬜🟩🟩  
        ⬜⬜🟩⬜🟩🟩  
        🟩🟩🟩🟩🟩🟩", 4, 6, 53)]
    [InlineData(@"https://ordlig.se https://ordlig.se WEEKEND nr 40, 4/6
        ⬜⬜🟩⬜🟩⬜  
        ⬜⬜🟩⬜🟩🟩  
        ⬜⬜🟩⬜🟩🟩  
        🟩🟩🟩🟩🟩🟩", 4, 6, 40)]
    [InlineData(@"http://ordlig.se WEEKEND nr 50, 2/6
        ⬜⬜🟩⬜🟩⬜  
        ⬜⬜🟩⬜🟩🟩  
        ⬜⬜🟩⬜🟩🟩  
        🟩🟩🟩🟩🟩🟩", 2, 6, 50)]
    [InlineData(@"http://ordlig.se nr 51, ❌/6
        ⬜⬜⬜⬜⬜  
        ⬜⬜⬜⬜⬜  
        🟩⬜⬜⬜⬜  
        🟩⬜⬜⬜⬜  
        ⬜⬜🟨⬜🟨  
        🟩⬜🟨🟨⬜", 0, 6, 51)]
    [InlineData(@"ordlig.se nr 61, 5/6

          ⬜⬜⬜⬜  
        ⬜⬜⬜  ⬜  
        ⬜⬜        
        ⬜⬜  ", 5, 6, 61)]
    public void Can_parse_Ordlig(string input, int expectedCompletion, int expectedRounds, int expectedGameRound)
    {
        var result = RoundHelper.GetRound(input);
        Assert.Equal(expectedCompletion, result?.CompletionRound);
        Assert.Equal(expectedRounds, result?.Rounds);
        Assert.Equal(expectedGameRound, result?.GameRound);
        Assert.Equal(GameType.Ordlig, result?.Type);
    }
}