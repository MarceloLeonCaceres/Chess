using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
public class Bishop : Piece
{
    public override PieceType Type => PieceType.Bishop;
    private readonly List<Desplazamiento> _desplazamientos;
    public Bishop(Color color) : base(color)
    {
        _desplazamientos = new List<Desplazamiento>
        {
            Desplazamiento.DiagonalUL,
            Desplazamiento.DiagonalUR,
            Desplazamiento.DiagonalDL,
            Desplazamiento.DiagonalDR
        };
    }
    public override List<Desplazamiento> Desplazamientos => _desplazamientos;

}
