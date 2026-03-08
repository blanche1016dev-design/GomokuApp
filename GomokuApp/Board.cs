namespace GomokuApp;

public sealed class Board
{
    private readonly CellState[,] _cells;

    public Board(int size = 15)
    {
        Size = size;
        _cells = new CellState[size, size];
    }

    public int Size { get; }

    public CellState this[int row, int column]
    {
        get => _cells[row, column];
        set => _cells[row, column] = value;
    }

    public bool IsInside(int row, int column) => row >= 0 && row < Size && column >= 0 && column < Size;

    public bool IsEmpty(int row, int column) => _cells[row, column] == CellState.Empty;

    public bool IsFull()
    {
        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                if (_cells[row, column] == CellState.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public IEnumerable<Move> GetAllEmptyCells()
    {
        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                if (_cells[row, column] == CellState.Empty)
                {
                    yield return new Move(row, column);
                }
            }
        }
    }

    public Board Clone()
    {
        var copy = new Board(Size);
        Array.Copy(_cells, copy._cells, _cells.Length);
        return copy;
    }

    public IReadOnlyList<Move> GetCandidateMoves(int radius = 2)
    {
        var moves = new HashSet<Move>();
        var hasStone = false;

        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                if (_cells[row, column] == CellState.Empty)
                {
                    continue;
                }

                hasStone = true;

                for (var dy = -radius; dy <= radius; dy++)
                {
                    for (var dx = -radius; dx <= radius; dx++)
                    {
                        var nextRow = row + dy;
                        var nextColumn = column + dx;
                        if (IsInside(nextRow, nextColumn) && IsEmpty(nextRow, nextColumn))
                        {
                            moves.Add(new Move(nextRow, nextColumn));
                        }
                    }
                }
            }
        }

        if (!hasStone)
        {
            var center = Size / 2;
            return [new Move(center, center)];
        }

        return moves
            .OrderBy(move => Math.Abs(move.Row - Size / 2) + Math.Abs(move.Column - Size / 2))
            .ToList();
    }
}
