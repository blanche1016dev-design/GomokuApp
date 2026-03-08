using GomokuApp;

namespace GomokuApp.Tests;

public sealed class GameSessionTests
{
    [Fact]
    public void StartNew_InitializesHumanAndCpuColors()
    {
        var session = new GameSession();

        session.StartNew(Difficulty.Normal, CellState.White);

        Assert.Equal(Difficulty.Normal, session.Difficulty);
        Assert.Equal(CellState.White, session.HumanColor);
        Assert.Equal(CellState.Black, session.CpuColor);
        Assert.Equal(CellState.Black, session.CurrentTurn);
        Assert.False(session.IsGameOver);
    }

    [Fact]
    public void TryPlayHumanMove_OnHumansTurn_PlacesStoneAndSwitchesTurn()
    {
        var session = new GameSession();
        session.StartNew(Difficulty.Easy, CellState.Black);

        var played = session.TryPlayHumanMove(7, 7);

        Assert.True(played);
        Assert.Equal(CellState.Black, session.Board[7, 7]);
        Assert.Equal(CellState.White, session.CurrentTurn);
        Assert.Equal(new Move(7, 7), session.LastMove);
    }

    [Fact]
    public void TryPlayHumanMove_OnOccupiedCell_ReturnsFalse()
    {
        var session = new GameSession();
        session.StartNew(Difficulty.Easy, CellState.Black);
        session.TryPlayHumanMove(7, 7);
        session.PlayCpuMove(7, 8);

        var played = session.TryPlayHumanMove(7, 7);

        Assert.False(played);
    }

    [Fact]
    public void TryPlayHumanMove_WhenCompletingFive_SetsWinner()
    {
        var session = new GameSession();
        session.StartNew(Difficulty.Easy, CellState.Black);

        session.Board[10, 2] = CellState.Black;
        session.Board[10, 3] = CellState.Black;
        session.Board[10, 4] = CellState.Black;
        session.Board[10, 5] = CellState.Black;

        var played = session.TryPlayHumanMove(10, 6);

        Assert.True(played);
        Assert.True(session.IsGameOver);
        Assert.Equal(CellState.Black, session.Winner);
    }
}
