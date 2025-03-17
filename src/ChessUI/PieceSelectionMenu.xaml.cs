using ChessLogic;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessUI;
/// <summary>
/// Interaction logic for PieceSelectionMenu.xaml
/// </summary>
public partial class PieceSelectionMenu : UserControl
{
    public event Action<PieceType, Player> PieceSelected;
    public event Action ClearSquareRequested;

    public event Action Closed;
    public Player CurrentPlayer => WhiteRadio.IsChecked == true ? Player.White : Player.Black;

    public PieceSelectionMenu(Player player)
    {
        InitializeComponent();

        // Set images for pieces
        PawnImg.Source = Images.GetImage(Player.White, PieceType.Pawn);
        KnightImg.Source = Images.GetImage(Player.White, PieceType.Knight);
        BishopImg.Source = Images.GetImage(Player.White, PieceType.Bishop);
        RookImg.Source = Images.GetImage(Player.White, PieceType.Rook);
        QueenImg.Source = Images.GetImage(Player.White, PieceType.Queen);
        KingImg.Source = Images.GetImage(Player.White, PieceType.King);

        this.Unloaded += (s, e) => Closed?.Invoke();
    }

    private void PieceImg_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Image image && image.Tag is string pieceTypeStr)
        {
            // Parse the piece type from the Tag
            PieceType pieceType = (PieceType)Enum.Parse(typeof(PieceType), pieceTypeStr);

            // Get the selected color
            Player color = WhiteRadio.IsChecked == true ? Player.White : Player.Black;

            // Trigger the event
            PieceSelected?.Invoke(pieceType, color);
        }
    }

    private void ClearSquare_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ClearSquareRequested?.Invoke();
    }

}
