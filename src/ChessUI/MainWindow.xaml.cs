using ChessLogic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChessUI;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Image[,] _pieceImages = new Image[8, 8];
    private readonly Rectangle[,] _highlights = new Rectangle[8, 8];
    private readonly Dictionary<Square, Move> _moveCache = new Dictionary<Square, Move>();

    private GameState _gameState;
    private Square _selectedCasilla = null;

    private bool _isFlipped = false;

    public MainWindow()
    {
        InitializeComponent();
        InitializeBoard();

        _gameState = new GameState(Player.White, Board.Initial());
        DrawBoard(_gameState.Board);
        SetCursor(_gameState.CurrentPlayer);
    }

    private void InitializeBoard()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Image image = new Image();
                _pieceImages[r, c] = image;
                PieceGrid.Children.Add(image);

                Rectangle highlight = new Rectangle();
                _highlights[r, c] = highlight;
                HighlightGrid.Children.Add(highlight);
            }
        }
    }

    private void DrawBoard(Board board)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                // Determine the actual board position based on flip state
                int boardRow = _isFlipped ? 7 - r : r;
                int boardCol = _isFlipped ? 7 - c : c;

                Piece piece = board[r, c];
                _pieceImages[r, c].Source = Images.GetImage(piece);
            }
        }
    }

    private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (IsMenuOnScreen())
        {
            return; // Ignore clicks if a menu is open
        }

        Point point = e.GetPosition(BoardGrid);
        Square pos = ToSquarePosition(point);

        if (IsSettingPositionMode)
        {
            HighlightSquare(pos);
            // Handle piece placement in position-setting mode
            ShowPieceSelectionMenu(pos);
        }
        else
        {
            // Handle normal move logic
            if (_selectedCasilla == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }
    }

    private void HighlightSquare(Square square)
    {
        ClearHighlights();

        int uiRow = _isFlipped ? 7 - square.Row : square.Row;
        int uiCol = _isFlipped ? 7 - square.Column : square.Column;

        _highlights[uiRow, uiCol].Fill = new SolidColorBrush(Colors.Yellow);
    }
    private void ClearHighlights()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                _highlights[r, c].Fill = Brushes.Transparent;
            }
        }
    }
    private void OnToPositionSelected(Square pos)
    {
        _selectedCasilla = null;
        HideHighlights();

        if (_moveCache.TryGetValue(pos, out Move move))
        {
            if(move.Type == MoveType.PawnPromotion)
            {
                HandlePromotion(move.FromCasilla, move.ToCasilla);
            }
            else
            {
                HandleMove(move);
            }
        }
    }

    private void HandlePromotion(Square from, Square to)
    {
        _pieceImages[to.Row, to.Column].Source = Images.GetImage(_gameState.CurrentPlayer, PieceType.Pawn);
        _pieceImages[from.Row, from.Column].Source = null;

        PromotionMenu promMenu = new PromotionMenu(_gameState.CurrentPlayer);
        MenuContainer.Content = promMenu;

        promMenu.PieceSelected += type =>
        {
            MenuContainer.Content = null;
            Move promMove = new PawnPromotion(from, to, type);
            HandleMove(promMove);
        };
    }

    private void HandleMove(Move move)
    {
        _gameState.MakeMove(move);
        DrawBoard(_gameState.Board);
        SetCursor(_gameState.CurrentPlayer);

        if (_gameState.IsGameOver())
        {
            ShowGameOver();
        }
    }

    private void OnFromPositionSelected(Square pos)
    {
        IEnumerable<Move> moves = _gameState.LegalMovesForPiece(pos);
        if (moves.Any())
        {
            _selectedCasilla = pos;
            CacheMoves(moves);
            ShowHighlights();
        }
    }

    private Square ToSquarePosition(Point point)
    {
        // Calculate the scaling factor introduced by the Viewbox
        double scaleX = BoardGrid.ActualWidth / 600; // 600 is the original width of the BoardGrid
        double scaleY = BoardGrid.ActualHeight / 600; // 600 is the original height of the BoardGrid

        // Adjust the mouse position for scaling
        double adjustedX = point.X / scaleX;
        double adjustedY = point.Y / scaleY;

        // Calculate the square size based on the original BoardGrid size
        double squareSize = 600 / 8; // Each square is 75x75 in the original size
        int row = (int)(adjustedY / squareSize);
        int col = (int)(adjustedX / squareSize);

        // Invert coordinates if board is flipped
        if (_isFlipped)
        {
            row = 7 - row;
            col = 7 - col;
        }

        return new Square(row, col);
    }
    private void CacheMoves(IEnumerable<Move> moves)
    {
        _moveCache.Clear();
        foreach (Move move in moves)
        {
            _moveCache[move.ToCasilla] = move;
        }
    }

    private void ShowHighlights()
    {
        Color color = Color.FromArgb(150, 125, 255, 125);
        foreach (Square to in _moveCache.Keys)
        {
            var (uiRow, uiCol) = ToUiPosition(to);
            _highlights[uiRow, uiCol].Fill = new SolidColorBrush(color);
        }
    }

    private void HideHighlights()
    {
        foreach (Square to in _moveCache.Keys)
        {
            var (uiRow, uiCol) = ToUiPosition(to);
            _highlights[uiRow, uiCol].Fill = Brushes.Transparent;
        }
    }
    public void ForceHideAllHighlights()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                _highlights[r, c].Fill = Brushes.Transparent;
            }
        }
        _moveCache.Clear();
        _selectedCasilla = null;
    }
    private void SetCursor(Player player)
    {
        if (player == Player.White)
        {
            Cursor = ChessCursors.WhiteCursor;
        }
        else if (player == Player.Black)
        {
            Cursor = ChessCursors.BlackCursor;
        }
    }

    private bool IsMenuOnScreen()
    {
        return MenuContainer.Content != null;
    }

    private void ShowGameOver()
    {
        GameOverMenu gameOverMenu = new GameOverMenu(_gameState);
        MenuContainer.Content = gameOverMenu;

        gameOverMenu.OptionSelected += option =>
        {
            if (option == Option.Restart)
            {
                MenuContainer.Content = null;
                RestartGame();
            }
            else
            {
                Application.Current.Shutdown();
            }
        };
    }

    private void RestartGame()
    {
        _selectedCasilla = null;
        HideHighlights();
        _moveCache.Clear();
        _gameState = new GameState(Player.White, Board.Initial());
        DrawBoard(_gameState.Board);
        SetCursor(_gameState.CurrentPlayer);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if(!IsMenuOnScreen() && e.Key == Key.Escape)
        {
            ShowPauseMenu();
        }
        else if(e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            UndoLastMove();
        }
    }

    private void ShowPauseMenu()
    {
        PauseMenu pauseMenu = new PauseMenu();
        MenuContainer.Content = pauseMenu;

        pauseMenu.OptionSelected += option =>
        {
            MenuContainer.Content = null;

            if(option == Option.Restart)
            {
                RestartGame();
            }
            else if(option == Option.Flip)
            {
                FlipBoardTablero();
            }
            else if (option == Option.Undo)
            {
                UndoLastMove();
            }
        };
    }

    private void UndoLastMove()
    {
        if (_gameState.UndoMove())
        {
            DrawBoard(_gameState.Board);
            SetCursor(_gameState.CurrentPlayer);
        }
        else
        {
            MessageBox.Show("No hay movidas para regresar.", "Regresar movimientos.", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private (int uiRow, int uiCol) ToUiPosition(Square chessSquare)
    {
        return (chessSquare.Row, chessSquare.Column);
    }

    private void FlipBoardTablero()
    {
        _isFlipped = !_isFlipped;

        ForceHideAllHighlights();

        // Reverse the order of the PieceGrid and HighlightGrid children
        ReverseGridChildren(PieceGrid);
        ReverseGridChildren(HighlightGrid);

        // Reverse row labels (1-8 <-> 8-1)
        ReverseRowLabels();

        // Reverse column labels (a-h <-> h-a)
        ReverseColumnLabels();
    }

    private void ReverseGridChildren(Panel grid)
    {
        var children = grid.Children.Cast<UIElement>().ToList();
        grid.Children.Clear();
        children.Reverse();  // Reverse the order
        foreach (var child in children)
        {
            grid.Children.Add(child);
        }
    }
    private void ReverseRowLabels()
    {
        var rowLabels = RowLabels.Children.Cast<TextBlock>().ToList();
        RowLabels.Children.Clear();
        rowLabels.Reverse(); // Flip the row labels (1-8 <-> 8-1)
        for (int i = 0; i < rowLabels.Count; i++)
        {
            Grid.SetRow(rowLabels[i], i);
            RowLabels.Children.Add(rowLabels[i]);
        }
    }

    private void ReverseColumnLabels()
    {
        var colLabels = ColumnLabels.Children.Cast<TextBlock>().ToList();
        ColumnLabels.Children.Clear();
        colLabels.Reverse(); // Flip the column labels (a-h <-> h-a)
        for (int i = 0; i < colLabels.Count; i++)
        {
            Grid.SetColumn(colLabels[i], i);
            ColumnLabels.Children.Add(colLabels[i]);
        }
    }


    private void ShowPieceSelectionMenu(Square square)
    {
        var pieceSelectionMenu = new PieceSelectionMenu(Player.White);
        MenuContainer.Content = pieceSelectionMenu;

        pieceSelectionMenu.PieceSelected += (pieceType, color) =>
        {
            // Place the selected piece on the board
            Piece piece = Piece.Create(pieceType, color);
            _gameState.Board[square.Row, square.Column] = piece;

            // Redraw the board
            DrawBoard(_gameState.Board);

            // Close the menu
            MenuContainer.Content = null;
        };

        pieceSelectionMenu.ClearSquareRequested += () =>
        {
            _gameState.Board[square.Row, square.Column] = null;
            DrawBoard(_gameState.Board);
            MenuContainer.Content = null;
        };

        pieceSelectionMenu.Closed += () =>
        {
            _gameState.CurrentPlayer = pieceSelectionMenu.CurrentPlayer;
            SetCursor(_gameState.CurrentPlayer);
            DrawBoard(_gameState.Board);
        };
    }


    private bool IsSettingPositionMode = false;

    private void TogglePositionSettingMode(object sender, RoutedEventArgs e)
    {
        IsSettingPositionMode = !IsSettingPositionMode;

        this.Background = IsSettingPositionMode
            ? new SolidColorBrush(Colors.LightGray)
            : new SolidColorBrush(Colors.Black);

        var setPositionButton = (Button)sender;
        setPositionButton.Content = IsSettingPositionMode ? "Set Position Off" : "Set Position On";

        if (IsSettingPositionMode)
        {
            MessageBox.Show("Position-setting mode enabled. \nClick on squares to place pieces.", 
                "Modo armar posicion", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            ClearHighlights();
            MessageBox.Show("Position-setting mode disabled.", 
                "Modo armar posicion", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    private void ClearBoard(object sender, RoutedEventArgs e)
    {
        _gameState.Board.Clear();

        // Reset game state
        _gameState = new GameState(Player.White, _gameState.Board);

        DrawBoard(_gameState.Board);

        if (IsSettingPositionMode)
        {
            this.Background = new SolidColorBrush(Colors.Black);
            IsSettingPositionMode = false;
            SetPositionButton.Content = "Set Position On";
        }

        // Reset UI elements
        SetCursor(_gameState.CurrentPlayer);
        HideHighlights();
        ClearHighlights();
    }
    private void SavePosition(string filePath)
    {
        string positionString = new StateString(_gameState.CurrentPlayer, _gameState.Board).ToString();
        File.WriteAllText(filePath, positionString);
    }
    private void LoadPosition(string filePath)
    {
        string positionString = File.ReadAllText(filePath);
        // Implement logic to parse the position string and set up the board
    }

    private void NewGame(object sender, RoutedEventArgs e)
    {
        RestartGame();
    }
}