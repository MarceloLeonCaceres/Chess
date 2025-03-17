namespace ChessLogic;
public abstract class Piece
{
    public abstract PieceType Type { get; }
    public abstract Player Color { get; }
    public bool HasMoved { get; set; } = false;
    public abstract Piece Copy();

    // Factory method to create pieces
    public static Piece Create(PieceType type, Player color)
    {
        return type switch
        {
            PieceType.Pawn => new Pawn(color),
            PieceType.Knight => new Knight(color),
            PieceType.Bishop => new Bishop(color),
            PieceType.Rook => new Rook(color),
            PieceType.Queen => new Queen(color),
            PieceType.King => new King(color),
            _ => throw new ArgumentException("Invalid piece type"),
        };
    }
    public abstract IEnumerable<Move> GetMoves(Square from, Board board);
    protected IEnumerable<Square> MovePositionInDir(Square from, Board board, Direction dir)
    {
        for(Square cas = from + dir; Board.IsInside(cas); cas += dir)
        {
            if (board.IsEmpty(cas))
            {
                yield return cas;
                continue;
            }
            
            Piece piece = board[cas];
            
            if(piece.Color != Color)
            {
                yield return cas;
            }

            yield break;
        }
    }
    protected IEnumerable<Square> MovePositionsInDirs(Square from, Board board, Direction[] dirs)
    {
        return dirs.SelectMany(dir => MovePositionInDir(from, board, dir));
    }

    public virtual bool CanCaptureOpponentKing(Square from, Board board)
    {
        return GetMoves(from, board).Any(move =>
        {
            Piece piece = board[move.ToCasilla];
            return piece != null && piece.Type == PieceType.King;
        });
    }
}
