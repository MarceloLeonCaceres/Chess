namespace ChessLogic;

public class EnPassant : Move
{
    public override MoveType Type => MoveType.EnPassant;

    public override Square FromPos { get; }

    public override Square ToPos { get; }

    private readonly Square capturePos;
    public EnPassant(Square from, Square to)
    {
        FromPos = from;
        ToPos = to;
        capturePos = new Square(from.Row, to.Column);
    }
    public override bool Execute(Board board)
    {
        new NormalMove(FromPos, ToPos).Execute(board);
        board[capturePos] = null;

        return true;
    }
}
