using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
public class Queen : Piece
{
    public override PieceType Type => PieceType.Queen;
    private readonly List<Desplazamiento> _desplazamientos;

    public Queen(Color color) : base(color)
    {
        _desplazamientos = new List<Desplazamiento>
        {
            Desplazamiento.Up,
            Desplazamiento.Down,
            Desplazamiento.Right,
            Desplazamiento.Left,
            Desplazamiento.DiagonalUL,
            Desplazamiento.DiagonalUR,
            Desplazamiento.DiagonalDL,
            Desplazamiento.DiagonalDR
        };
    }

    public override List<Desplazamiento> Desplazamientos => _desplazamientos;

}