namespace GomokuApp;

public sealed class GameSession
{
    public Board Board { get; private set; } = new();
    public Difficulty Difficulty { get; private set; }
    public CellState HumanColor { get; private set; } = CellState.Black;
    public CellState CpuColor { get; private set; } = CellState.White;
    public CellState CurrentTurn { get; private set; } = CellState.Black;
    public Move? LastMove { get; private set; }
    public bool IsGameOver { get; private set; }
    public CellState? Winner { get; private set; }
    public Move? RecommendedMove { get; set; }

    public void StartNew(Difficulty difficulty, CellState humanColor)
    {
        Board = new Board();
        Difficulty = difficulty;
        HumanColor = humanColor;
        CpuColor = humanColor.Opponent();
        CurrentTurn = CellState.Black;
        LastMove = null;
        IsGameOver = false;
        Winner = null;
        RecommendedMove = null;
    }

    public bool TryPlayHumanMove(int row, int column)
    {
        if (IsGameOver || CurrentTurn != HumanColor || !Board.IsInside(row, column) || !Board.IsEmpty(row, column))
        {
            return false;
        }

        PlaceMove(row, column, HumanColor);
        return true;
    }

    public void PlayCpuMove(int row, int column)
    {
        if (IsGameOver || CurrentTurn != CpuColor || !Board.IsInside(row, column) || !Board.IsEmpty(row, column))
        {
            return;
        }

        PlaceMove(row, column, CpuColor);
    }

    private void PlaceMove(int row, int column, CellState color)
    {
        Board[row, column] = color;
        LastMove = new Move(row, column);
        RecommendedMove = null;

        if (GameRuleService.IsWinningMove(Board, row, column, color))
        {
            IsGameOver = true;
            Winner = color;
            return;
        }

        if (Board.IsFull())
        {
            IsGameOver = true;
            Winner = null;
            return;
        }

        CurrentTurn = color.Opponent();
    }
}
