
using ConsoleApp.Moves;

namespace ConsoleApp;
public class Casilla
{
    public char Columna { get; }
    public int Fila { get; }
    public Color Color { get; init; }
    public Casilla(char columna, int fila)
    {
        char column = char.ToLower(columna);
        Fila = fila;
        Columna = column;
        Color = SetColor(column, fila);
    }

    public Casilla? To(Desplazamiento direccion)
    {
        char columnaDestino = (char)(Columna + direccion.ColumnDelta);
        int filaDestino = Fila + direccion.RowDelta;
        if (!Tablero.EstaEnElTablero(columnaDestino, filaDestino))
        {
            return null;
        }
        return new Casilla(columnaDestino, filaDestino);
    }

    private static Color SetColor(char columna, int fila)
    {
        if((fila + columna) % 2 == 0)
        {
            return Color.Black;
        }
        return Color.White;
    }
    public override string ToString()
    {
        return $"{Columna}{Fila} ({Color})";
    }
}
