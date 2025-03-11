namespace ConsoleApp.Moves;
public abstract class Move
{
    public MoveType Type { get; set; }  
    public Casilla Desde { get; set; }
    public Casilla Hasta{ get; set; }
    


}
