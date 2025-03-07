namespace ChessLogic;

public class EnPassant : Move
{
    public override MoveType Type => MoveType.EnPassant;

    public override Square FromCasilla { get; }

    public override Square ToCasilla { get; }

    private readonly Square capturePos;
    public EnPassant(Square from, Square to)
    {
        FromCasilla = from;
        ToCasilla = to;
        capturePos = new Square(from.Row, to.Column);
    }
    public override bool Execute(Board board)
    {
        new NormalMove(FromCasilla, ToCasilla).Execute(board);
        board[capturePos] = null;

        return true;
    }
}
