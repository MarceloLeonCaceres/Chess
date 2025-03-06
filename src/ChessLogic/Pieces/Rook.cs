namespace ChessLogic;
public class Rook : Piece
{
    public override PieceType Type => PieceType.Rook;

    public override Player Color { get; }
    private static readonly Direction[] dirs = new Direction[]
    {
        Direction.East,
        Direction.West,
        Direction.North,
        Direction.South
    };
    public Rook(Player color)
    {
            Color = color;
    }
    public override Piece Copy()
    {
        Rook copy = new Rook(Color);
        copy.HasMoved = HasMoved;
        return copy;
    }
    public override IEnumerable<Move> GetMoves(Square from, Board board)
    {
        return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
