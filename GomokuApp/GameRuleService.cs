namespace GomokuApp;

public static class GameRuleService
{
    private static readonly (int Row, int Column)[] Directions =
    [
        (0, 1),
        (1, 0),
        (1, 1),
        (1, -1)
    ];

    public static bool IsWinningMove(Board board, int row, int column, CellState color)
    {
        foreach (var (dy, dx) in Directions)
        {
            var count = 1 + CountDirection(board, row, column, dy, dx, color) + CountDirection(board, row, column, -dy, -dx, color);
            if (count >= 5)
            {
                return true;
            }
        }

        return false;
    }

    public static CellState? GetWinner(Board board)
    {
        for (var row = 0; row < board.Size; row++)
        {
            for (var column = 0; column < board.Size; column++)
            {
                var color = board[row, column];
                if (color != CellState.Empty && IsWinningMove(board, row, column, color))
                {
                    return color;
                }
            }
        }

        return null;
    }

    public static IEnumerable<Move> FindImmediateWinningMoves(Board board, CellState color)
    {
        foreach (var move in board.GetCandidateMoves())
        {
            var next = board.Clone();
            next[move.Row, move.Column] = color;
            if (IsWinningMove(next, move.Row, move.Column, color))
            {
                yield return move;
            }
        }
    }

    private static int CountDirection(Board board, int row, int column, int dy, int dx, CellState color)
    {
        var count = 0;
        var currentRow = row + dy;
        var currentColumn = column + dx;

        while (board.IsInside(currentRow, currentColumn) && board[currentRow, currentColumn] == color)
        {
            count++;
            currentRow += dy;
            currentColumn += dx;
        }

        return count;
    }
}
