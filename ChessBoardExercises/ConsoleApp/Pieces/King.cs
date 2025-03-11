using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
internal class King : Piece
{
    public override PieceType Type => PieceType.King;
    private readonly List<Desplazamiento> _desplazamientos;
    public King(Color color) : base(color)
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