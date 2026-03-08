namespace GomokuApp;

public sealed class AiEngine
{
    private readonly Random _random = new();

    public Move ChooseMove(Board board, CellState aiColor, Difficulty difficulty)
    {
        var immediateWins = GameRuleService.FindImmediateWinningMoves(board, aiColor).ToList();
        if (immediateWins.Count > 0)
        {
            return immediateWins[0];
        }

        var blockMoves = GameRuleService.FindImmediateWinningMoves(board, aiColor.Opponent()).ToList();
        if (blockMoves.Count > 0)
        {
            return blockMoves[0];
        }

        var candidates = board.GetCandidateMoves();
        if (difficulty == Difficulty.Hard)
        {
            return ChooseByMinimax(board, aiColor, candidates);
        }

        var scored = candidates
            .Select(move => new ScoredMove(move, ScoreMove(board, move, aiColor, difficulty)))
            .OrderByDescending(item => item.Score)
            .ToList();

        if (difficulty == Difficulty.Easy && scored.Count > 1)
        {
            var top = scored.Take(Math.Min(3, scored.Count)).ToList();
            return top[_random.Next(top.Count)].Move;
        }

        return scored[0].Move;
    }

    public int EvaluateBoard(Board board, CellState perspective)
    {
        var winner = GameRuleService.GetWinner(board);
        if (winner == perspective)
        {
            return 1_000_000;
        }

        if (winner == perspective.Opponent())
        {
            return -1_000_000;
        }

        var own = EvaluatePatterns(board, perspective);
        var opp = EvaluatePatterns(board, perspective.Opponent());
        return own - opp;
    }

    private Move ChooseByMinimax(Board board, CellState aiColor, IReadOnlyList<Move> candidates)
    {
        var bestScore = int.MinValue;
        var bestMove = candidates[0];

        foreach (var move in candidates)
        {
            var next = board.Clone();
            next[move.Row, move.Column] = aiColor;
            var score = Minimax(next, depth: 2, maximizing: false, currentColor: aiColor.Opponent(), perspective: aiColor, alpha: int.MinValue, beta: int.MaxValue);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(Board board, int depth, bool maximizing, CellState currentColor, CellState perspective, int alpha, int beta)
    {
        if (depth == 0 || board.IsFull() || GameRuleService.GetWinner(board) is not null)
        {
            return EvaluateBoard(board, perspective);
        }

        var candidates = board.GetCandidateMoves(radius: 1);
        if (maximizing)
        {
            var best = int.MinValue;
            foreach (var move in candidates)
            {
                var next = board.Clone();
                next[move.Row, move.Column] = currentColor;
                best = Math.Max(best, Minimax(next, depth - 1, false, currentColor.Opponent(), perspective, alpha, beta));
                alpha = Math.Max(alpha, best);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return best;
        }

        var worst = int.MaxValue;
        foreach (var move in candidates)
        {
            var next = board.Clone();
            next[move.Row, move.Column] = currentColor;
            worst = Math.Min(worst, Minimax(next, depth - 1, true, currentColor.Opponent(), perspective, alpha, beta));
            beta = Math.Min(beta, worst);
            if (beta <= alpha)
            {
                break;
            }
        }

        return worst;
    }

    private int ScoreMove(Board board, Move move, CellState aiColor, Difficulty difficulty)
    {
        var next = board.Clone();
        next[move.Row, move.Column] = aiColor;

        var score = EvaluateBoard(next, aiColor);
        score += CenterBias(next.Size, move) * 8;

        if (difficulty == Difficulty.Easy)
        {
            score -= EvaluateRisk(board, move, aiColor) / 2;
        }
        else
        {
            score -= EvaluateRisk(board, move, aiColor);
        }

        return score;
    }

    private static int CenterBias(int size, Move move)
    {
        var center = size / 2;
        return Math.Max(0, 12 - (Math.Abs(move.Row - center) + Math.Abs(move.Column - center)));
    }

    private int EvaluateRisk(Board board, Move move, CellState aiColor)
    {
        var next = board.Clone();
        next[move.Row, move.Column] = aiColor;
        var opponentWins = GameRuleService.FindImmediateWinningMoves(next, aiColor.Opponent()).Count();
        return opponentWins * 50_000;
    }

    private int EvaluatePatterns(Board board, CellState color)
    {
        var score = 0;
        for (var row = 0; row < board.Size; row++)
        {
            for (var column = 0; column < board.Size; column++)
            {
                if (board[row, column] != color)
                {
                    continue;
                }

                score += EvaluateStone(board, row, column, color);
            }
        }

        return score;
    }

    private static int EvaluateStone(Board board, int row, int column, CellState color)
    {
        var score = 0;
        var directions = new (int Row, int Column)[] { (0, 1), (1, 0), (1, 1), (1, -1) };

        foreach (var (dy, dx) in directions)
        {
            var length = 1;
            var openEnds = 0;

            length += Count(board, row, column, dy, dx, color, ref openEnds);
            length += Count(board, row, column, -dy, -dx, color, ref openEnds);
            score += PatternScore(length, openEnds);
        }

        return score;
    }

    private static int Count(Board board, int row, int column, int dy, int dx, CellState color, ref int openEnds)
    {
        var length = 0;
        var currentRow = row + dy;
        var currentColumn = column + dx;

        while (board.IsInside(currentRow, currentColumn) && board[currentRow, currentColumn] == color)
        {
            length++;
            currentRow += dy;
            currentColumn += dx;
        }

        if (board.IsInside(currentRow, currentColumn) && board[currentRow, currentColumn] == CellState.Empty)
        {
            openEnds++;
        }

        return length;
    }

    private static int PatternScore(int length, int openEnds)
    {
        if (length >= 5) return 200_000;
        if (length == 4 && openEnds == 2) return 40_000;
        if (length == 4 && openEnds == 1) return 12_000;
        if (length == 3 && openEnds == 2) return 4_500;
        if (length == 3 && openEnds == 1) return 1_200;
        if (length == 2 && openEnds == 2) return 500;
        if (length == 2 && openEnds == 1) return 120;
        return 20;
    }

    private readonly record struct ScoredMove(Move Move, int Score);
}
