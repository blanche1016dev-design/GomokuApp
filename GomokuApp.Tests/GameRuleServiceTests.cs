using GomokuApp;

namespace GomokuApp.Tests;

public sealed class GameRuleServiceTests
{
    [Fact]
    public void IsWinningMove_HorizontalFive_ReturnsTrue()
    {
        var board = new Board();
        for (var column = 3; column <= 7; column++)
        {
            board[5, column] = CellState.Black;
        }

        var result = GameRuleService.IsWinningMove(board, 5, 5, CellState.Black);

        Assert.True(result);
    }

    [Fact]
    public void IsWinningMove_DiagonalFive_ReturnsTrue()
    {
        var board = new Board();
        for (var i = 0; i < 5; i++)
        {
            board[2 + i, 4 + i] = CellState.White;
        }

        var result = GameRuleService.IsWinningMove(board, 4, 6, CellState.White);

        Assert.True(result);
    }

    [Fact]
    public void GetWinner_WithoutFiveInARow_ReturnsNull()
    {
        var board = BoardTestHelper.CreateBoard(
            (7, 7, CellState.Black),
            (7, 8, CellState.Black),
            (7, 9, CellState.Black),
            (8, 8, CellState.White),
            (8, 9, CellState.White));

        var winner = GameRuleService.GetWinner(board);

        Assert.Null(winner);
    }

    [Fact]
    public void FindImmediateWinningMoves_WhenSingleGapExists_ReturnsThatGap()
    {
        var board = BoardTestHelper.CreateBoard(
            (7, 5, CellState.Black),
            (7, 6, CellState.Black),
            (7, 7, CellState.Black),
            (7, 8, CellState.Black));

        var moves = GameRuleService.FindImmediateWinningMoves(board, CellState.Black).ToList();

        Assert.Contains(new Move(7, 4), moves);
        Assert.Contains(new Move(7, 9), moves);
    }
}
