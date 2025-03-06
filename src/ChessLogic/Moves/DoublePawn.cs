namespace ChessLogic;

public class DoublePawn : Move
{
    public override MoveType Type => MoveType.DoublePawn;

    public override Square FromPos { get; }

    public override Square ToPos { get; }

    private readonly Square skippedPos;
    public DoublePawn(Square from, Square to)
    {
        FromPos = from;
        ToPos = to;
        skippedPos = new Square((from.Row + to.Row) / 2, from.Column);
    }

    public override bool Execute(Board board)
    {
        Player player = board[FromPos].Color;
        board.SetPawnSkipPosition(player, skippedPos);
        new NormalMove(FromPos, ToPos).Execute(board);

        return true;
    }
}
