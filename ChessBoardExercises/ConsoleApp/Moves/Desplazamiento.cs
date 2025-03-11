namespace ConsoleApp.Moves;
public class Desplazamiento
{
    public int RowDelta { get; }
    public int ColumnDelta { get; }
    public Desplazamiento(int rowDelta, int columnaDelta)
    {
        RowDelta = rowDelta;
        ColumnDelta = columnaDelta;
    }

    public readonly static Desplazamiento Up = new Desplazamiento(1, 0);
    public readonly static Desplazamiento Down = new Desplazamiento(-1, 0);
    public readonly static Desplazamiento Right = new Desplazamiento(0, 1);
    public readonly static Desplazamiento Left = new Desplazamiento(0, -1);

    public static Desplazamiento operator + (Desplazamiento dir1, Desplazamiento dir2)
    {
        return new Desplazamiento(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
    }
    public static Desplazamiento operator * (int scalar, Desplazamiento dir)
    {
        return new Desplazamiento(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
    }

    public readonly static Desplazamiento DiagonalUR = Up + Right;
    public readonly static Desplazamiento DiagonalUL = Up + Left;
    public readonly static Desplazamiento DiagonalDR = Down + Right;
    public readonly static Desplazamiento DiagonalDL = Down + Left;

}
