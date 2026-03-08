using System.Drawing.Drawing2D;

namespace GomokuApp;

public partial class Form1 : Form
{
    private const int CellSize = 36;
    private const int BoardMargin = 24;

    private readonly GameSession _session = new();
    private readonly AiEngine _aiEngine = new();
    private readonly HintService _hintService = new();
    private bool _cpuBusy;

    public Form1()
    {
        InitializeComponent();
        boardPanel.DoubleBuffered(true);
        difficultyComboBox.SelectedIndex = 0;
        turnComboBox.SelectedIndex = 0;
        StartNewGame();
    }

    private async void StartButton_Click(object? sender, EventArgs e)
    {
        StartNewGame();
        await ProcessCpuTurnIfNeededAsync();
    }

    private async void ResetButton_Click(object? sender, EventArgs e)
    {
        StartNewGame();
        await ProcessCpuTurnIfNeededAsync();
    }

    private void BoardPanel_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Color.FromArgb(239, 200, 122));

        using var gridPen = new Pen(Color.FromArgb(110, 78, 36), 1.4f);
        var boardPixels = (_session.Board.Size - 1) * CellSize;

        for (var i = 0; i < _session.Board.Size; i++)
        {
            var offset = BoardMargin + i * CellSize;
            e.Graphics.DrawLine(gridPen, BoardMargin, offset, BoardMargin + boardPixels, offset);
            e.Graphics.DrawLine(gridPen, offset, BoardMargin, offset, BoardMargin + boardPixels);
        }

        if (_session.RecommendedMove is { } hint)
        {
            var hintRect = GetStoneBounds(hint.Row, hint.Column);
            using var hintBrush = new SolidBrush(Color.FromArgb(96, 38, 166, 154));
            using var hintPen = new Pen(Color.FromArgb(16, 104, 94), 3f);
            e.Graphics.FillEllipse(hintBrush, hintRect);
            e.Graphics.DrawEllipse(hintPen, hintRect);
        }

        for (var row = 0; row < _session.Board.Size; row++)
        {
            for (var column = 0; column < _session.Board.Size; column++)
            {
                var state = _session.Board[row, column];
                if (state == CellState.Empty)
                {
                    continue;
                }

                var stoneRect = GetStoneBounds(row, column);
                using var brush = new SolidBrush(state == CellState.Black ? Color.FromArgb(32, 32, 32) : Color.FromArgb(244, 244, 244));
                using var pen = new Pen(state == CellState.Black ? Color.Black : Color.Gray, 1.4f);
                e.Graphics.FillEllipse(brush, stoneRect);
                e.Graphics.DrawEllipse(pen, stoneRect);

                if (_session.LastMove is { Row: var lastRow, Column: var lastColumn } && row == lastRow && column == lastColumn)
                {
                    using var lastPen = new Pen(Color.OrangeRed, 3f);
                    e.Graphics.DrawEllipse(lastPen, stoneRect);
                }
            }
        }
    }

    private async void BoardPanel_MouseClick(object? sender, MouseEventArgs e)
    {
        if (_cpuBusy || _session.IsGameOver || _session.CurrentTurn != _session.HumanColor)
        {
            return;
        }

        if (!TryGetBoardPosition(e.Location, out var row, out var column))
        {
            return;
        }

        if (!_session.TryPlayHumanMove(row, column))
        {
            return;
        }

        RefreshUi();
        ShowResultIfFinished();
        await ProcessCpuTurnIfNeededAsync();
    }

    private async Task ProcessCpuTurnIfNeededAsync()
    {
        if (_session.IsGameOver || _session.CurrentTurn != _session.CpuColor || _cpuBusy)
        {
            return;
        }

        _cpuBusy = true;
        RefreshUi();

        await Task.Delay(_session.Difficulty switch
        {
            Difficulty.Easy => 180,
            Difficulty.Normal => 320,
            _ => 550
        });

        var move = _aiEngine.ChooseMove(_session.Board, _session.CpuColor, _session.Difficulty);
        _session.PlayCpuMove(move.Row, move.Column);

        _cpuBusy = false;
        RefreshUi();
        ShowResultIfFinished();
    }

    private void ShowResultIfFinished()
    {
        if (!_session.IsGameOver)
        {
            return;
        }

        var message = _session.Winner switch
        {
            null => "引き分けです。",
            var winner when winner == _session.HumanColor => "あなたの勝ちです。",
            _ => "CPU の勝ちです。"
        };

        MessageBox.Show(this, message, "対局結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void StartNewGame()
    {
        var difficulty = difficultyComboBox.SelectedIndex switch
        {
            0 => Difficulty.Easy,
            1 => Difficulty.Normal,
            _ => Difficulty.Hard
        };

        var humanColor = turnComboBox.SelectedIndex == 0 ? CellState.Black : CellState.White;
        _session.StartNew(difficulty, humanColor);
        _cpuBusy = false;
        RefreshUi();
    }

    private void RefreshUi()
    {
        statusLabel.Text = _session.IsGameOver
            ? (_session.Winner is null ? "状態: 引き分け" : $"状態: {(_session.Winner == _session.HumanColor ? "あなたの勝ち" : "CPU の勝ち")}")
            : $"状態: {(_session.CurrentTurn == _session.HumanColor ? "あなたの手番" : "CPU 思考中")}";

        difficultyValueLabel.Text = _session.Difficulty.ToString();
        hintLabel.Text = _session.RecommendedMove is null
            ? "推奨手: なし"
            : $"推奨手: {_session.RecommendedMove.Value.Column + 1}, {_session.RecommendedMove.Value.Row + 1}";
        hintDetailLabel.Text = _session.RecommendedMove is null
            ? "Easy の危険局面でのみ表示されます。"
            : "この位置に置くと相手の強い連続を防ぎやすいです。";

        if (!_session.IsGameOver && _session.CurrentTurn == _session.HumanColor && _session.Difficulty == Difficulty.Easy)
        {
            _session.RecommendedMove = _hintService.GetRecommendedMove(_session.Board, _session.HumanColor, _session.CpuColor);
            hintLabel.Text = _session.RecommendedMove is null
                ? "推奨手: なし"
                : $"推奨手: {_session.RecommendedMove.Value.Column + 1}, {_session.RecommendedMove.Value.Row + 1}";
            hintDetailLabel.Text = _session.RecommendedMove is null
                ? "Easy の危険局面でのみ表示されます。"
                : "この位置に置くと相手の強い連続を防ぎやすいです。";
        }
        else if (_session.Difficulty != Difficulty.Easy || _session.CurrentTurn != _session.HumanColor)
        {
            _session.RecommendedMove = null;
            hintLabel.Text = "推奨手: なし";
            hintDetailLabel.Text = _session.Difficulty == Difficulty.Easy
                ? "あなたの手番で危険局面になると表示されます。"
                : "Easy でのみ推奨手を表示します。";
        }

        boardPanel.Invalidate();
    }

    private Rectangle GetStoneBounds(int row, int column)
    {
        var centerX = BoardMargin + column * CellSize;
        var centerY = BoardMargin + row * CellSize;
        return new Rectangle(centerX - 14, centerY - 14, 28, 28);
    }

    private bool TryGetBoardPosition(Point point, out int row, out int column)
    {
        row = -1;
        column = -1;

        var max = BoardMargin + (_session.Board.Size - 1) * CellSize;
        if (point.X < BoardMargin - CellSize / 2 || point.Y < BoardMargin - CellSize / 2 || point.X > max + CellSize / 2 || point.Y > max + CellSize / 2)
        {
            return false;
        }

        column = (int)Math.Round((point.X - BoardMargin) / (double)CellSize);
        row = (int)Math.Round((point.Y - BoardMargin) / (double)CellSize);

        return row >= 0 && row < _session.Board.Size && column >= 0 && column < _session.Board.Size;
    }
}

internal static class ControlExtensions
{
    public static void DoubleBuffered(this Control control, bool enabled)
    {
        typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(control, enabled, null);
    }
}
