namespace ChessLogic;
public abstract class Move
{
    public abstract MoveType Type { get; }
    public abstract Square FromPos { get; }
    public abstract Square ToPos { get; }
    public abstract bool Execute(Board board);

    public virtual bool IsLegal(Board board)
    {
        Player player = board[FromPos].Color;
        Board boardCopy = board.Copy();
        Execute(boardCopy);
        return !boardCopy.IsInCheck(player);
    }
}
