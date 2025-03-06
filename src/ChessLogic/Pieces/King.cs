namespace ChessLogic;
public class King : Piece
{
    public override PieceType Type => PieceType.King;
    public override Player Color { get; }
    private static readonly Direction[] _dirs = new Direction[]
    {
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West,
        Direction.NorthEast,
        Direction.SouthEast,
        Direction.NorthWest,
        Direction.SouthWest
    };
    public King(Player color)
    {
        Color = color;
    }

    private static bool IsUnmovedRook(Square pos, Board board)
    {
        if (board.IsEmpty(pos))
        {
            return false;
        }

        Piece piece = board[pos];
        return piece.Type == PieceType.Rook && !piece.HasMoved;
    }

    private static bool AllEmpty(IEnumerable<Square> positions, Board board)
    {
        return positions.All(pos => board.IsEmpty(pos));
    }

    private bool CanCastleKingSide(Square from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }
        Square rookPos = new Square(from.Row, 7);
        Square[] betweenPositions = new Square[] { new(from.Row, 5), new(from.Row, 6) };
        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);

    }

    private bool CanCastleQueenSide(Square from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }
        Square rookPos = new Square(from.Row, 0);
        Square[] betweenPositions = new Square[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };
        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);

    }

    public override Piece Copy()
    {
        King copy = new King(Color);
        copy.HasMoved = HasMoved;
        return copy;
    }
    private IEnumerable<Square> MovePositions(Square from, Board board)
    {
        foreach(var dir in _dirs)
        {
            Square to = from + dir;
            if (!Board.IsInside(to))
            {
                continue;
            }
            if(board.IsEmpty(to) || board[to].Color != Color)
            {
                yield return to;
            }
        }
    }

    public override IEnumerable<Move> GetMoves(Square from, Board board)
    {
        foreach(Square to in MovePositions(from, board))
        {
            yield return new NormalMove(from, to);
        }

        if(CanCastleKingSide(from, board))
        {
            yield return new Castle(MoveType.CastleKS, from);
        }
        if (CanCastleQueenSide(from, board))
        {
            yield return new Castle(MoveType.CastleQS, from);
        }
    }
    public override bool CanCaptureOpponentKing(Square from, Board board)
    {
        return MovePositions(from, board).Any(to =>
        {
            Piece piece = board[to];
            return piece != null && piece.Type == PieceType.King;
        });
    }

}
