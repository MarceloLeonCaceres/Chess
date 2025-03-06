using System.Runtime.CompilerServices;
using System.Text;

namespace ChessLogic;
public class StateString
{
    private readonly StringBuilder _sb = new StringBuilder();

    public StateString(Player currentPlayer, Board board)
    {
        AddPiecePlacement(board);
        _sb.Append(' ');
        AddCurrentPlayer(currentPlayer);
        _sb.Append(' ');
        AddCastlingRights(board);
        _sb.Append(' ');
        AddEnPassant(board, currentPlayer);
    }
    public override string ToString()
    {
        return _sb.ToString();
    }
    private static char PieceChar(Piece piece)
    {
        char c = piece.Type switch
        {
            PieceType.Pawn => 'p',
            PieceType.Knight => 'n',
            PieceType.Rook => 'r',
            PieceType.Bishop => 'b',
            PieceType.Queen => 'q',
            PieceType.King => 'k',
            _ => ' '
        };

        if(piece.Color == Player.White)
        {
            return char.ToUpper(c);
        }

        return c;
    }

    private void AddRowData(Board board, int row)
    {
        int empty = 0;

        for(int c = 0; c < 8; c++)
        {
            if (board[row, c] == null)
            {
                empty++;
                continue;
            }

            if(empty > 0)
            {
                _sb.Append(empty);
                empty = 0;
            }

            _sb.Append(PieceChar(board[row, c]));   
        }

        if(empty > 0)
        {
            _sb.Append(empty);
        }
    }

    private void AddPiecePlacement(Board board)
    {
        for (int r =0; r < 8; r++)
        {
            if(r!= 0)
            {
                _sb.Append('/');
            }

            AddRowData(board, r);
        }
    }

    private void AddCurrentPlayer(Player currentPlayer)
    {
        if(currentPlayer == Player.White)
        {
            _sb.Append('w');
        }
        else
        {
            _sb.Append('b');
        }
    }

    private void AddCastlingRights(Board board)
    {
        bool castleWKS = board.CastleRightKS(Player.White);
        bool castleWQS = board.CastleRightKS(Player.White);
        bool castleBKS = board.CastleRightKS(Player.Black);
        bool castleBQS = board.CastleRightKS(Player.Black);

        if(!(castleWKS || castleWQS || castleBKS || castleBQS))
        {
            _sb.Append('-');
            return;
        }

        if (castleWKS)
        {
            _sb.Append('K');
        }
        if (castleWQS)
        {
            _sb.Append('Q');
        }
        if (castleBKS)
        {
            _sb.Append('k');
        }
        if (castleBQS)
        {
            _sb.Append('q');
        }
    }

    private void AddEnPassant(Board board, Player currentPlayer)
    {
        if (!board.CanCaptureEnPassant(currentPlayer))
        {
            _sb.Append('-');
            return;
        }

        Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());
        char file = (char)('a' + pos.Column);
        int rank = 8 - pos.Row;
        _sb.Append(file);
        _sb.Append(rank);
    }

}
