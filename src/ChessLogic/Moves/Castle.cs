namespace ChessLogic;
public class Castle : Move
{
    public override MoveType Type { get; }

    public override Square FromPos { get; }

    public override Square ToPos { get; }

    private readonly Direction kingMoveDir;
    private readonly Square rookFromPos;
    private readonly Square rookToPos;

    public Castle(MoveType type, Square kingPos)
    {
        Type = type;
        FromPos = kingPos;
            
        if(type == MoveType.CastleKS)
        {
            kingMoveDir = Direction.East;
            ToPos = new Square(kingPos.Row, 6);
            rookFromPos = new Square(kingPos.Row, 7);
            rookToPos = new Square(kingPos.Row, 5);
        }
        else
        {
            kingMoveDir = Direction.West;
            ToPos = new Square(kingPos.Row, 2);
            rookFromPos = new Square(kingPos.Row, 0);
            rookToPos = new Square(kingPos.Row, 3);
        }
    }
    public override bool Execute(Board board)
    {
        new NormalMove(FromPos, ToPos).Execute(board);
        new NormalMove(rookFromPos, rookToPos).Execute(board);

        return false;
    }
    public override bool IsLegal(Board board)
    {
        Player player = board[FromPos].Color;

        if (board.IsInCheck(player))
        {
            return false;
        }

        Board copy = board.Copy();
        Square kingPosInCopy = FromPos;

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
