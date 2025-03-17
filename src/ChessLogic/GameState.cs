using System.Collections.Generic;

namespace ChessLogic;
public class GameState
{
    public Board Board { get; }
    public Player CurrentPlayer { get; set; }
    public Result Result { get; private set; } = null;

    private int _noCaptureOrPawnMoves = 0;
    private string _stateString;
    private readonly Dictionary<string, int> _stateHistory = new Dictionary<string, int>();
    private readonly Stack<(Board Board, Player CurrentPlayer, string StateString)> _moveHistory = new();

    public GameState(Player player, Board board)
    {
        CurrentPlayer = player;
        Board = board;

        _stateString = new StateString(CurrentPlayer, board).ToString();
        _stateHistory[_stateString] = 1;
    }
    public IEnumerable<Move> LegalMovesForPiece(Square pos)
    {
        if(Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
        {
            return Enumerable.Empty<Move>();
        }

        Piece piece = Board[pos];
        IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
        return moveCandidates.Where(move => move.IsLegal(Board));
    }

    public void MakeMove(Move move)
    {
        _moveHistory.Push((Board.Copy(), CurrentPlayer, _stateString));

        Board.SetPawnSkipPosition(CurrentPlayer, null);
        bool captureOrPawn = move.Execute(Board);

        if (captureOrPawn)
        {
            _noCaptureOrPawnMoves = 0;
            // Borra la historia ?!
            _stateHistory.Clear();
        }
        else
        {
            _noCaptureOrPawnMoves++;
        }

        CurrentPlayer = CurrentPlayer.Opponent();
        UpdateStateString();
        CheckForGameOver();
    }
    public bool UndoMove()
    {
        if (_moveHistory.Count == 0)
        {
            return false; // No moves to undo
        }

        // Pop the previous state from the stack
        var (previousBoard, previousPlayer, previousStateString) = _moveHistory.Pop();

        // Restore the previous state
        Board.CopyFrom(previousBoard);
        CurrentPlayer = previousPlayer;
        _stateString = previousStateString;

        // Update state history
        if (!_stateHistory.ContainsKey(_stateString))
        {
            _stateHistory[_stateString] = 1;
        }
        else
        {
            _stateHistory[_stateString]++;
        }

        return true;
    }
    public IEnumerable<Move> AllLegalMovesFor(Player player)
    {
        IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
        {
            Piece piece = Board[pos];
            return piece.GetMoves(pos, Board);
        });

        return moveCandidates.Where(move => move.IsLegal(Board));
    }

    private void CheckForGameOver()
    {
        if (!AllLegalMovesFor(CurrentPlayer).Any())
        {
            if(Board.IsInCheck(CurrentPlayer)) 
            {
                Result = Result.Win(CurrentPlayer.Opponent());
            }
            else
            {
                Result = Result.Draw(EndReason.Stalemate);
            }
        }
        else if (Board.InsufficientMaterial())
        {
            Result = Result.Draw(EndReason.InsufficientMaterial);
        }
        else if(FiftyMoveRule())
        {
            Result = Result.Draw(EndReason.FiftyMoveRule);
        }
        else if (ThreefoldRepetition())
        {
            Result = Result.Draw(EndReason.ThreefoldRepetition);
        }
    }

    public bool IsGameOver()
    {
        return Result != null;
    }

    private bool FiftyMoveRule()
    {
        int fullMoves = _noCaptureOrPawnMoves / 2;
        return fullMoves == 50;
    }

    private void UpdateStateString()
    {
        _stateString = new StateString(CurrentPlayer, Board).ToString();
        if (!_stateHistory.ContainsKey(_stateString))
        {
            _stateHistory[_stateString] = 1;
        }
        else
        {
            _stateHistory[_stateString]++;
        }
    }

    private bool ThreefoldRepetition()
    {
        return _stateHistory[_stateString] == 3;
    }
}
