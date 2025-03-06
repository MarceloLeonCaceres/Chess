namespace ChessLogic;
public class Square
{
    public int Row { get; }
    public int Column { get; }
    public Square(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public Player SquareColor()
    {
        if ((Row + Column) % 2 == 0)
        {
            return Player.White;
        }
        return Player.Black;
    }

    public override bool Equals(object obj)
    {
        return obj is Square position &&
               Row == position.Row &&
               Column == position.Column;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }

    public static bool operator ==(Square left, Square right)
    {
        return EqualityComparer<Square>.Default.Equals(left, right);
    }

    public static bool operator !=(Square left, Square right)
    {
        return !(left == right);
    }
    public static Square operator + (Square pos, Direction dir)
    {
        return new Square(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
    }
}
