namespace ChessLogic;

public class DoublePawn : Move
{
    public override MoveType Type => MoveType.DoublePawn;

    public override Square FromCasilla { get; }

    public override Square ToCasilla { get; }

    private readonly Square _skippedCasilla;
    public DoublePawn(Square from, Square to)
    {
        FromCasilla = from;
        ToCasilla = to;
        _skippedCasilla = new Square((from.Row + to.Row) / 2, from.Column);
    }

    public override bool Execute(Board board)
    {
        Player player = board[FromCasilla].Color;
        board.SetPawnSkipPosition(player, _skippedCasilla);
        new NormalMove(FromCasilla, ToCasilla).Execute(board);

        return true;
    }
}
