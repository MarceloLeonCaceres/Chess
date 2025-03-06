namespace ChessLogic;
public class NormalMove : Move
{
    public override MoveType Type => MoveType.Normal;

    public override Square FromPos { get; }

    public override Square ToPos { get; }

    public NormalMove(Square from, Square to)
    {
        FromPos = from;
        ToPos = to;
    }
    public override bool Execute(Board board)
    {
        Piece piece = board[FromPos];
        bool capture = !board.IsEmpty(ToPos);
        board[ToPos] = piece;
        board[FromPos] = null;
        piece.HasMoved = true;

        return capture || piece.Type == PieceType.Pawn;
    }
}
