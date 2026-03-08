using GomokuApp;

namespace GomokuApp.Tests;

public sealed class HintServiceTests
{
    [Fact]
    public void GetRecommendedMove_WhenCpuHasImmediateWin_ReturnsBlockingMove()
    {
        var board = BoardTestHelper.CreateBoard(
            (8, 4, CellState.White),
            (8, 5, CellState.White),
            (8, 6, CellState.White),
            (8, 7, CellState.White),
            (7, 7, CellState.Black));
        var service = new HintService();

        var recommended = service.GetRecommendedMove(board, CellState.Black, CellState.White);

        Assert.NotNull(recommended);
        Assert.Contains(recommended.Value, new[] { new Move(8, 3), new Move(8, 8) });
    }

    [Fact]
    public void GetRecommendedMove_WhenBoardIsCalm_ReturnsNull()
    {
        var board = BoardTestHelper.CreateBoard(
            (7, 7, CellState.Black),
            (7, 8, CellState.White),
            (8, 7, CellState.Black));
        var service = new HintService();

        var recommended = service.GetRecommendedMove(board, CellState.Black, CellState.White);

        Assert.Null(recommended);
    }
}
