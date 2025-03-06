using ChessLogic;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI;
/// <summary>
/// Interaction logic for GameOverMenu.xaml
/// </summary>
public partial class GameOverMenu : UserControl
{
    public event Action<Option> OptionSelected;
    public GameOverMenu(GameState gameState)
    {
        InitializeComponent();

        Result result = gameState.Result;
        WinnerText.Text = GetWinnerText(result.Winner);
        ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
    }

    private static string GetWinnerText(Player winner)
    {
        return winner switch
        { 
            Player.White => "BLANCAS GANA!",
            Player.Black => "NEGRAS GANAN!",
            _ => "EMPATE!" 
        };
    }

    private static string PlayerString(Player player)
    {
        return player switch
        {
            Player.White => "BLANCAS",
            Player.Black => "NEGRAS",
            _ => ""
        };
    }

    private static string GetReasonText(EndReason reason, Player currentPlayer)
    {
        return reason switch
        {
            EndReason.Stalemate => $"Ahogado - {PlayerString(currentPlayer)} No puede mover",
            EndReason.Checkmate => $"JaqueMate - {PlayerString(currentPlayer)} No puede mover",
            EndReason.FiftyMoveRule => $"Regla de 50 movimientos",
            EndReason.InsufficientMaterial => $"Material Insuficiente",
            EndReason.ThreefoldRepetition => $"Posicion Repetida 3 veces",
            _ => ""
        };
    }

    private void Restart_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Restart);
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Exit);
    }
}
