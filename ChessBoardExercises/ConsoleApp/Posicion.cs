using ConsoleApp.Pieces;

namespace ConsoleApp;
public class Posicion
{
    private readonly Dictionary<Casilla, Piece> _pieces = new Dictionary<Casilla, Piece>();

    public Posicion SetPosicion()
    {
        _pieces[new Casilla('b', 7)] = new Rook(Color.White);

        return this;
    }

    public List<Casilla> PosiblesMovimientos(Casilla casilla, Piece piece)
    {
        List<Casilla> posibles = new List<Casilla>();
        Casilla? next;

        foreach (var direccion in piece.Desplazamientos)
        {
            if(piece.Type == PieceType.Knight || piece.Type == PieceType.King)
            {
                if ((next = casilla.To(direccion)) != null && Tablero.EstaEnElTablero(next))
                {
                    posibles.Add(next);
                }
            }
            else
            {
                int i = 1;
                while ((next = casilla.To(i * direccion)) != null && Tablero.EstaEnElTablero(next))
                {
                    posibles.Add(next);
                    i++;
                }
            }
        }
        return posibles;
    }

}
