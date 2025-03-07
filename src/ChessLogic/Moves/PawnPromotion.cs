namespace ChessLogic;
public class PawnPromotion : Move
{
    public override MoveType Type => MoveType.PawnPromotion;

    public override Square FromCasilla { get; }

    public override Square ToCasilla { get; }

    private readonly PieceType newType;
    public PawnPromotion(Square from, Square to, PieceType newType)
    {
        FromCasilla = from;
        ToCasilla = to;
        this.newType = newType;
    }
    private Piece CreatePromotionPiece(Player color)
    {
        return newType switch
        {
            PieceType.Knight => new Knight(color),
            PieceType.Bishop => new Bishop(color),
            PieceType.Rook => new Rook(color),
            _ => new Queen(color)
        };
    }
    public override bool Execute(Board board)
    {
        Piece pawn = board[FromCasilla];
        board[FromCasilla] = null;

        Piece promotionPiece = CreatePromotionPiece(pawn.Color);
        promotionPiece.HasMoved = true;
        board[ToCasilla] = promotionPiece;

        return true;
    }
}
