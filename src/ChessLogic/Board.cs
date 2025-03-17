namespace ChessLogic;
public class Board
{
    private readonly Piece[,] _pieces = new Piece[8, 8];

    private readonly Dictionary<Player, Square> _pawnSkipPositions = new Dictionary<Player, Square>
    {
        {Player.White, null },
        {Player.Black, null }
    };
    public Piece this[int row, int col]
    {
        get { return _pieces[row, col]; }
        set { _pieces[row, col] = value; }
    }

    public Square GetPawnSkipPosition(Player player)
    {
        return _pawnSkipPositions[player];
    }
    public void SetPawnSkipPosition(Player player, Square casilla)
    {
        _pawnSkipPositions[player] = casilla;
    }

    public Piece this[Square casilla]
    {
        get { return this[casilla.Row, casilla.Column]; }
        set { this[casilla.Row, casilla.Column] = value; }
    }

    public static Board Initial()
    {
        Board board = new Board();
        board.AddStartPieces();
        //board.Ejercicio_1();
        return board;
    }
    public void Ejercicio_1()
    {
        this[0, 2] = new Bishop(Player.Black);
        this[0, 4] = new King(Player.Black);
        this[1, 0] = new King(Player.White);
        this[2, 2] = new Pawn(Player.White);
        this[2, 3] = new Pawn(Player.White);
    }
    private void AddStartPieces()
    {
        this[0, 0] = new Rook(Player.Black);
        this[0, 1] = new Knight(Player.Black);
        this[0, 2] = new Bishop(Player.Black);
        this[0, 3] = new Queen(Player.Black);
        this[0, 4] = new King(Player.Black);
        this[0, 5] = new Bishop(Player.Black);
        this[0, 6] = new Knight(Player.Black);
        this[0, 7] = new Rook(Player.Black);

        this[7, 0] = new Rook(Player.White);
        this[7, 1] = new Knight(Player.White);
        this[7, 2] = new Bishop(Player.White);
        this[7, 3] = new Queen(Player.White);
        this[7, 4] = new King(Player.White);
        this[7, 5] = new Bishop(Player.White);
        this[7, 6] = new Knight(Player.White);
        this[7, 7] = new Rook(Player.White);

        for (int i = 0; i < 8; i++)
        {
            this[1, i] = new Pawn(Player.Black);
            this[6, i] = new Pawn(Player.White);
        }
    }

    public static bool IsInside(Square casilla)
    {
        return casilla.Row >= 0 && casilla.Row < 8 && casilla.Column >= 0 && casilla.Column < 8;
    }

    public bool IsEmpty(Square casilla)
    {
        return this[casilla] == null;
    }

    public IEnumerable<Square> PiecePositions()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Square casilla = new Square(r, c);

                if (!IsEmpty(casilla))
                {
                    yield return casilla;
                }
            }
        }
    }

    public IEnumerable<Square> PiecePositionsFor(Player player)
    {
        return PiecePositions().Where(cas => this[cas].Color == player);
    }

    public bool IsInCheck(Player player)
    {
        return PiecePositionsFor(player.Opponent()).Any(cas =>
        {
            Piece piece = this[cas];
            return piece.CanCaptureOpponentKing(cas, this);
        });
    }

    public Board Copy()
    {
        Board copy = new Board();

        foreach (Square casilla in PiecePositions())
        {
            copy[casilla] = this[casilla].Copy();
        }
        return copy;
    }

    public void CopyFrom(Board other)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                this[r, c] = other[r, c];
            }
        }

        // Copy additional board state (e.g., castling rights, en passant)
        //this.CastleRightKS(Player.White) = other.CastleRightKS(Player.White);
        //this.CastleRightQSWhite = other.CastleRightQSWhite;
        //this.CastleRightKSBlack = other.CastleRightKSBlack;
        //this.CastleRightQSBlack = other.CastleRightQSBlack;
        //this.PawnSkipPositionWhite = other.PawnSkipPositionWhite;
        //this.PawnSkipPositionBlack = other.PawnSkipPositionBlack;
    }

    public Counting CountPieces()
    {
        Counting counting = new Counting();

        foreach (var casilla in PiecePositions())
        {
            Piece piece = this[casilla];
            counting.Increment(piece.Color, piece.Type);
        }

        return counting;
    }

    public bool InsufficientMaterial()
    {
        Counting counting = CountPieces();

        return IsKingVKing(counting) || IsKingBishopVKing(counting) ||
            IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
    }

    private static bool IsKingVKing(Counting counting)
    {
        return counting.TotalCount == 2;
    }

    private static bool IsKingBishopVKing(Counting counting)
    {
        return counting.TotalCount == 3 &&
            (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
    }

    private static bool IsKingKnightVKing(Counting counting)
    {
        return counting.TotalCount == 3 &&
            (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
    }
    private bool IsKingBishopVKingBishop(Counting counting)
    {
        if (counting.TotalCount != 4)
        {
            return false;
        }

        if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
        {
            return false;
        }

        Square wBishopPos = FindPiece(Player.White, PieceType.Bishop);
        Square bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

        return wBishopPos.SquareColor() == bBishopPos.SquareColor();
    }

    private Square FindPiece(Player color, PieceType type)
    {
        return PiecePositionsFor(color).First(pos => this[pos].Type == type);
    }

    private bool IsUnmovedKingAndRook(Square kingPos, Square rookPos)
    {
        if (IsEmpty(kingPos) || IsEmpty(rookPos))
        {
            return false;
        }

        Piece king = this[kingPos];
        Piece rook = this[rookPos];

        return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;

    }

    public bool CastleRightKS(Player player)
    {
        return player switch
        {
            Player.White => IsUnmovedKingAndRook(new Square(7, 4), new Square(7, 7)),
            Player.Black => IsUnmovedKingAndRook(new Square(0, 4), new Square(0, 7)),
            _ => false
        };
    }
    public bool CastleRightQS(Player player)
    {
        return player switch
        {
            Player.White => IsUnmovedKingAndRook(new Square(7, 4), new Square(7, 0)),
            Player.Black => IsUnmovedKingAndRook(new Square(0, 4), new Square(0, 0)),
            _ => false
        };
    }

    private bool HasPawnInPosition(Player player, Square[] pawnPositions, Square skipPos)
    {
        foreach (Square casilla in pawnPositions.Where(IsInside))
        {
            Piece piece = this[casilla];
            if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
            {
                continue;
            }

            EnPassant move = new EnPassant(casilla, skipPos);
            if (move.IsLegal(this))
            {
                return true;
            }
        }

        return false;
    }
    public bool CanCaptureEnPassant(Player player)
    {
        Square skipCasilla = GetPawnSkipPosition(player.Opponent());
        if (skipCasilla == null)
        {
            return false;
        }

        Square[] pawnPositions = player switch
        {
            Player.White => new Square[] { skipCasilla + Direction.SouthWest, skipCasilla + Direction.SouthEast },
            Player.Black => new Square[] { skipCasilla + Direction.NorthWest, skipCasilla + Direction.NorthEast },
            _ => Array.Empty<Square>()
        };

        return HasPawnInPosition(player, pawnPositions, skipCasilla);
    }

    public void Clear()
    {
        for(int r = 0; r < 8; r++)
        {
            for(int c = 0; c < 8; c++)
            {
                _pieces[r, c] = null;
            }
        }
    }
}
