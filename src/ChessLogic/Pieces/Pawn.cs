
namespace ChessLogic;
public class Pawn : Piece
{
    public override PieceType Type => PieceType.Pawn;

    public override Player Color { get; }
    private readonly Direction _forward;
    public Pawn(Player color)
    {
        Color = color;
        if (color == Player.White)
        {
            _forward = Direction.North;
        }
        else if (color == Player.Black)
        {
            _forward = Direction.South;
        }
    }
    public override Piece Copy()
    {
        Pawn copy = new Pawn(Color);
        copy.HasMoved = HasMoved;
        return copy;
    }
    private static bool CanMoveTo(Square pos, Board board)
    {
        return Board.IsInside(pos) && board.IsEmpty(pos);
    }
    private bool CanCaptureAt(Square pos, Board board)
    {
        if (!Board.IsInside(pos) || board.IsEmpty(pos))
        {
            return false;
        }
        return board[pos].Color != Color;
    }

    private static IEnumerable<Move> PromotionMoves(Square from, Square to)
    {
        yield return new PawnPromotion(from, to, PieceType.Knight);
        yield return new PawnPromotion(from, to, PieceType.Bishop);
        yield return new PawnPromotion(from, to, PieceType.Rook);
        yield return new PawnPromotion(from, to, PieceType.Queen);
    }

    private IEnumerable<Move> ForwardsMove(Square from, Board board)
    {
        Square oneMovePos = from + _forward;
        if (CanMoveTo(oneMovePos, board))
        {
            if(oneMovePos.Row == 0 || oneMovePos.Row == 7)
            {
                foreach(Move promMove in PromotionMoves(from, oneMovePos))
                {
                    yield return promMove;
                }
            }
            else
            {
                yield return new NormalMove(from, oneMovePos);
            }

            Square twoMovesPos = oneMovePos + _forward;

            if (!HasMoved && CanMoveTo(twoMovesPos, board))
            {
                yield return new DoublePawn(from, twoMovesPos);
            }
        }
    }
    private IEnumerable<Move> DiagonalMoves(Square from, Board board)
    {
        foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
        {
            Square to = from + _forward + dir;

            if (to == board.GetPawnSkipPosition(Color.Opponent()))
            {
                yield return new EnPassant(from, to);   
            }
            else if (CanCaptureAt(to, board))
            {
                if (to.Row == 0 || to.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, to))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, to);
                }
            }
        }
    }
    public override IEnumerable<Move> GetMoves(Square from, Board board)
    {
        return ForwardsMove(from, board).Concat(DiagonalMoves(from, board));
    }

    public override bool CanCaptureOpponentKing(Square from, Board board)
    {
        return DiagonalMoves(from, board).Any(move =>
        {
            Piece piece = board[move.ToCasilla];
            return piece != null && piece.Type == PieceType.King;
        });
    }

}
