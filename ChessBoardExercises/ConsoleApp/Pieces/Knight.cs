using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
public class Knight : Piece
{
    public override PieceType Type => PieceType.Knight;
    private readonly List<Desplazamiento> _desplazamientos;
    public Knight(Color color) : base(color)
    {
        _desplazamientos = new List<Desplazamiento>
        {
            Desplazamiento.Up + 2 * Desplazamiento.Left,
            Desplazamiento.Up + 2 * Desplazamiento.Right,
            Desplazamiento.Down + 2 * Desplazamiento.Left,
            Desplazamiento.Down + 2 * Desplazamiento.Right,
            Desplazamiento.Left + 2 * Desplazamiento.Up,
            Desplazamiento.Left + 2 * Desplazamiento.Down,
            Desplazamiento.Right + 2 * Desplazamiento.Up,
            Desplazamiento.Right + 2 * Desplazamiento.Down
        };
    }
    public override List<Desplazamiento> Desplazamientos => _desplazamientos;
}
