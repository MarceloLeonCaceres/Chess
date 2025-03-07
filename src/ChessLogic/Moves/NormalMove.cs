namespace ChessLogic;
public class NormalMove : Move
{
    public override MoveType Type => MoveType.Normal;

    public override Square FromCasilla { get; }

    public override Square ToCasilla { get; }

    public NormalMove(Square from, Square to)
    {
        FromCasilla = from;
        ToCasilla = to;
    }
    public override bool Execute(Board board)
    {
        Piece piece = board[FromCasilla];
        bool capture = !board.IsEmpty(ToCasilla);
        board[ToCasilla] = piece;
        board[FromCasilla] = null;
        piece.HasMoved = true;

        return capture || piece.Type == PieceType.Pawn;
    }
}
