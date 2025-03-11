using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
public class Rook : Piece
{
    public override PieceType Type => PieceType.Rook;
    private readonly List<Desplazamiento> _desplazamientos;

    public Rook(Color color) : base(color)
    {
        _desplazamientos = new List<Desplazamiento>
        {
            Desplazamiento.Up,
            Desplazamiento.Down,
            Desplazamiento.Right,
            Desplazamiento.Left
        };
    }

    public override List<Desplazamiento> Desplazamientos => _desplazamientos;

}
