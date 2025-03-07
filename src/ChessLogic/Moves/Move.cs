namespace ChessLogic;
public abstract class Move
{
    public abstract MoveType Type { get; }
    public abstract Square FromCasilla { get; }
    public abstract Square ToCasilla { get; }
    public abstract bool Execute(Board board);

    public virtual bool IsLegal(Board board)
    {
        Player player = board[FromCasilla].Color;
        Board boardCopy = board.Copy();
        Execute(boardCopy);
        return !boardCopy.IsInCheck(player);
    }
}
