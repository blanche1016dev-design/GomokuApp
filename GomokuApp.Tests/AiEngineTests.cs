using GomokuApp;

namespace GomokuApp.Tests;

public sealed class AiEngineTests
{
    [Fact]
    public void ChooseMove_WhenAiCanWin_PicksWinningMove()
    {
        var board = BoardTestHelper.CreateBoard(
            (7, 3, CellState.White),
            (7, 4, CellState.White),
            (7, 5, CellState.White),
            (7, 6, CellState.White),
            (6, 5, CellState.Black),
            (8, 5, CellState.Black));
        var engine = new AiEngine();

        var move = engine.ChooseMove(board, CellState.White, Difficulty.Normal);

        Assert.Contains(move, new[] { new Move(7, 2), new Move(7, 7) });
    }

    [Fact]
    public void ChooseMove_WhenOpponentHasImmediateWin_BlocksIt()
    {
        var board = BoardTestHelper.CreateBoard(
            (5, 8, CellState.Black),
            (6, 8, CellState.Black),
            (7, 8, CellState.Black),
            (8, 8, CellState.Black),
            (7, 7, CellState.White));
        var engine = new AiEngine();

        var move = engine.ChooseMove(board, CellState.White, Difficulty.Hard);

        Assert.Contains(move, new[] { new Move(4, 8), new Move(9, 8) });
    }
}
