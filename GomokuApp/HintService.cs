namespace GomokuApp;

public sealed class HintService
{
    private readonly AiEngine _engine = new();

    public Move? GetRecommendedMove(Board board, CellState humanColor, CellState cpuColor)
    {
        var cpuWinningMoves = GameRuleService.FindImmediateWinningMoves(board, cpuColor).ToList();
        if (cpuWinningMoves.Count > 0)
        {
            return cpuWinningMoves[0];
        }

        var candidates = board.GetCandidateMoves();
        var dangerousReplies = new List<(Move Move, int Score)>();

        foreach (var move in candidates)
        {
            var next = board.Clone();
            next[move.Row, move.Column] = humanColor;
            var score = _engine.EvaluateBoard(next, cpuColor);
            dangerousReplies.Add((move, score));
        }

        var bestDefense = dangerousReplies.OrderBy(item => item.Score).FirstOrDefault();
        var worstCurrent = _engine.EvaluateBoard(board, cpuColor);

        if (dangerousReplies.Count == 0 || worstCurrent < 2_500)
        {
            return null;
        }

        return bestDefense.Score < worstCurrent ? bestDefense.Move : null;
    }
}
