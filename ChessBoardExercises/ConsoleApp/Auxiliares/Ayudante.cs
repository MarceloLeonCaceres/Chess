using ConsoleApp.Pieces;

namespace ConsoleApp.Auxiliares;
public class Ayudante
{
    public static void PrintPosibles(Casilla casilla, Piece pieza)
    {
        var posicion = new Posicion();

        var posibles = posicion.PosiblesMovimientos(casilla, pieza);

        Console.WriteLine();
        Console.WriteLine($"Posibles movimientos de una {pieza}, desde {casilla}");
        foreach (var celda in posibles)
        {
            Console.WriteLine(celda);
        }
    }

    public static void PrintTablero()
    {
        for (int fila = 8; fila >= 1; fila--)
        {
            for (char col = 'a'; col <= 'h'; col++)
            {
                var casilla = new Casilla(col, fila);
                Console.Write($"{casilla}  ");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
