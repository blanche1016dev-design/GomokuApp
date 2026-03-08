using GomokuApp;

namespace GomokuApp.Tests;

internal static class BoardTestHelper
{
    public static Board CreateBoard(params (int Row, int Column, CellState State)[] stones)
    {
        var board = new Board();
        foreach (var (row, column, state) in stones)
        {
            board[row, column] = state;
        }

        return board;
    }
}
