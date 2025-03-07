namespace ChessLogic;
public class Castle : Move
{
    public override MoveType Type { get; }

    public override Square FromCasilla { get; }

    public override Square ToCasilla { get; }

    private readonly Direction kingMoveDir;
    private readonly Square rookFromPos;
    private readonly Square rookToPos;

    public Castle(MoveType type, Square kingPos)
    {
        Type = type;
        FromCasilla = kingPos;
            
        if(type == MoveType.CastleKS)
        {
            kingMoveDir = Direction.East;
            ToCasilla = new Square(kingPos.Row, 6);
            rookFromPos = new Square(kingPos.Row, 7);
            rookToPos = new Square(kingPos.Row, 5);
        }
        else
        {
            kingMoveDir = Direction.West;
            ToCasilla = new Square(kingPos.Row, 2);
            rookFromPos = new Square(kingPos.Row, 0);
            rookToPos = new Square(kingPos.Row, 3);
        }
    }
    public override bool Execute(Board board)
    {
        new NormalMove(FromCasilla, ToCasilla).Execute(board);
        new NormalMove(rookFromPos, rookToPos).Execute(board);

        return false;
    }
    public override bool IsLegal(Board board)
    {
        Player player = board[FromCasilla].Color;

        if (board.IsInCheck(player))
        {
            return false;
        }

        Board copy = board.Copy();
        Square kingPosInCopy = FromCasilla;

        for(int i = 0; i < 2; i++)
        {
            new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(copy);
            kingPosInCopy += kingMoveDir;

            if (copy.IsInCheck(player))
            {
                return false;
            }
        }

        return true;
    }
}
