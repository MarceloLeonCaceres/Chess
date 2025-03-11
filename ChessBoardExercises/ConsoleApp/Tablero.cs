using ConsoleApp.Pieces;

namespace ConsoleApp;
public static class Tablero
{
    private static readonly Casilla[,] _casillas = new Casilla[8, 8];

    //public Tablero()
    //{
    //    for(char c = 'a'; c <= 'h'; c++)
    //    {
    //        for(int fila = 1; fila <= 8; fila++)
    //        {
    //            _casillas[c, fila] = new Casilla(c, fila);
    //        }
    //    }
    //}

    public static bool EstaEnElTablero(char columna, int fila)
    {
        if (fila < 1 || fila > 8)
        {
            return false;
        }
        char column = char.ToLower(columna);
        if (column < 'a' || column > 'h')
        {
            return false;
        }
        return true;
    }

    public static bool EstaEnElTablero(Casilla casilla)
    {
        if (casilla.Fila < 1 || casilla.Fila > 8)
        {
            return false;
        }
        char column = char.ToLower(casilla.Columna);
        if (column < 'a' || column > 'h')
        {
            return false;
        }
        return true;
    }

}
