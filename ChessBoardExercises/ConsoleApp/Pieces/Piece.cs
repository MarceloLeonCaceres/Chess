using ConsoleApp.Moves;

namespace ConsoleApp.Pieces;
public abstract class Piece
{
    public abstract PieceType Type { get; } 
    public Color Color { get; set; }
    public abstract List<Desplazamiento> Desplazamientos { get; }

    protected Piece(Color color)
    {
        Color = color;
    }

    public override string ToString()
    {
        return $"{Type}";
    }
}
