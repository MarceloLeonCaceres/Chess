namespace ChessLogic;
public abstract class Piece
{
    public abstract PieceType Type { get; }
    public abstract Player Color { get; }
    public bool HasMoved { get; set; } = false;
    public abstract Piece Copy();
    public abstract IEnumerable<Move> GetMoves(Square from, Board board);
    protected IEnumerable<Square> MovePositionInDir(Square from, Board board, Direction dir)
    {
        for(Square pos = from + dir; Board.IsInside(pos); pos += dir)
        {
            if (board.IsEmpty(pos))
            {
                yield return pos;
                continue;
            }
            
            Piece piece = board[pos];
            
            if(piece.Color != Color)
            {
                yield return pos;
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
            Piece piece = board[move.ToPos];
            return piece != null && piece.Type == PieceType.King;
        });
    }
}
