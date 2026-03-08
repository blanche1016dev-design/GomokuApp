namespace GomokuApp;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public enum CellState
{
    Empty,
    Black,
    White
}

public readonly record struct Move(int Row, int Column);

public static class CellStateExtensions
{
    public static CellState Opponent(this CellState value) =>
        value switch
        {
            CellState.Black => CellState.White,
            CellState.White => CellState.Black,
            _ => CellState.Empty
        };
}
