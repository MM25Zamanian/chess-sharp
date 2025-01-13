public class Board
{
    private readonly Piece[,] _squares;

    public Board()
    {
        _squares = new Piece[8, 8];
        SetupInitialBoard();
    }

    // سازنده کپی برای بررسی حرکت‌های غیرمجاز
    public Board(Board other)
    {
        _squares = new Piece[8, 8];
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                _squares[row, col] = other._squares[row, col];
            }
        }
    }

    public Piece GetPieceAt(Position position)
    {
        return _squares[position.Row, position.Col];
    }

    public void SetPieceAt(Position position, Piece? piece)
    {
        _squares[position.Row, position.Col] = piece;

        if (piece != null)
        {
            piece.Position = position;
        }
    }

    public void MovePiece(Position from, Position to)
    {
        Piece piece = GetPieceAt(from) ?? throw new ArgumentException("No piece at the starting position.");

        List<Position> possibleMoves = piece.GetPossibleMoves(this);

        if (!possibleMoves.Contains(to))
        {
            throw new ArgumentException("Invalid move for this piece.");
        }

        // بررسی اینکه حرکت منجر به کیش شدن شاه خودی می‌شود یا خیر
        Board tempBoard = new(this);
        tempBoard.MovePieceInternal(from, to);
        if (tempBoard.IsKingInCheck(piece.Color))
        {
            throw new ArgumentException("This move leaves your king in check.");
        }

        MovePieceInternal(from, to);
        piece.HasMoved = true;
    }

    // حرکت داخلی بدون بررسی کیش برای استفاده در بررسی کیش
    internal void MovePieceInternal(Position from, Position to)
    {
        Piece piece = GetPieceAt(from);
        SetPieceAt(from, null);
        SetPieceAt(to, piece);
    }

    // بررسی اینکه آیا یک خانه توسط مهره‌های یک رنگ خاص مورد حمله قرار می‌گیرد
    public bool IsSquareAttackedBy(Position position, Color attackingColor)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = GetPieceAt(new Position(row, col));
                if (piece != null && piece.Color == attackingColor)
                {
                    List<Position> possibleAttacks = piece.GetPossibleMoves(this);
                    if (possibleAttacks.Contains(position))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // پیدا کردن موقعیت شاه یک رنگ خاص
    private Position? GetKingPosition(Color color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = GetPieceAt(new Position(row, col));
                if (piece is King && piece.Color == color)
                {
                    return piece.Position;
                }
            }
        }
        return null;
    }

    // بررسی اینکه آیا شاه یک رنگ خاص در وضعیت کیش قرار دارد
    public bool IsKingInCheck(Color kingColor)
    {
        Position? kingPosition = GetKingPosition(kingColor);
        if (!kingPosition.HasValue)
        {
            return false; // شاه نباید از بین برود، اما برای اطمینان
        }

        Color opponentColor = kingColor == Color.Red ? Color.Green : Color.Red;
        return IsSquareAttackedBy(kingPosition.Value, opponentColor);
    }

    // بررسی اینکه آیا یک رنگ خاص کیش و مات شده است
    public bool IsCheckmate(Color playerColor)
    {
        if (!IsKingInCheck(playerColor))
        {
            return false;
        }

        // بررسی اینکه آیا هیچ حرکت قانونی برای خروج از کیش وجود دارد یا خیر
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = GetPieceAt(new Position(row, col));
                if (piece != null && piece.Color == playerColor)
                {
                    List<Position> possibleMoves = piece.GetPossibleMoves(this);
                    foreach (var move in possibleMoves)
                    {
                        Board tempBoard = new(this);
                        tempBoard.MovePieceInternal(piece.Position, move);
                        if (!tempBoard.IsKingInCheck(playerColor))
                        {
                            return false; // حرکتی وجود دارد که از کیش خارج شود
                        }
                    }
                }
            }
        }

        return true;
    }

    // تنظیم موقعیت اولیه مهره‌ها
    private void SetupInitialBoard()
    {
        // مهره‌های سفید
        SetPieceAt(new Position(1, 0), new Pawn(Color.Red, new Position(1, 0)));
        SetPieceAt(new Position(1, 1), new Pawn(Color.Red, new Position(1, 1)));
        SetPieceAt(new Position(1, 2), new Pawn(Color.Red, new Position(1, 2)));
        SetPieceAt(new Position(1, 3), new Pawn(Color.Red, new Position(1, 3)));
        SetPieceAt(new Position(1, 4), new Pawn(Color.Red, new Position(1, 4)));
        SetPieceAt(new Position(1, 5), new Pawn(Color.Red, new Position(1, 5)));
        SetPieceAt(new Position(1, 6), new Pawn(Color.Red, new Position(1, 6)));
        SetPieceAt(new Position(1, 7), new Pawn(Color.Red, new Position(1, 7)));
        SetPieceAt(new Position(0, 0), new Rook(Color.Red, new Position(0, 0)));
        SetPieceAt(new Position(0, 1), new Knight(Color.Red, new Position(0, 1)));
        SetPieceAt(new Position(0, 2), new Bishop(Color.Red, new Position(0, 2)));
        SetPieceAt(new Position(0, 3), new Queen(Color.Red, new Position(0, 3)));
        SetPieceAt(new Position(0, 4), new King(Color.Red, new Position(0, 4)));
        SetPieceAt(new Position(0, 5), new Bishop(Color.Red, new Position(0, 5)));
        SetPieceAt(new Position(0, 6), new Knight(Color.Red, new Position(0, 6)));
        SetPieceAt(new Position(0, 7), new Rook(Color.Red, new Position(0, 7)));

        // مهره‌های سیاه
        SetPieceAt(new Position(6, 0), new Pawn(Color.Green, new Position(6, 0)));
        SetPieceAt(new Position(6, 1), new Pawn(Color.Green, new Position(6, 1)));
        SetPieceAt(new Position(6, 2), new Pawn(Color.Green, new Position(6, 2)));
        SetPieceAt(new Position(6, 3), new Pawn(Color.Green, new Position(6, 3)));
        SetPieceAt(new Position(6, 4), new Pawn(Color.Green, new Position(6, 4)));
        SetPieceAt(new Position(6, 5), new Pawn(Color.Green, new Position(6, 5)));
        SetPieceAt(new Position(6, 6), new Pawn(Color.Green, new Position(6, 6)));
        SetPieceAt(new Position(6, 7), new Pawn(Color.Green, new Position(6, 7)));
        SetPieceAt(new Position(7, 0), new Rook(Color.Green, new Position(7, 0)));
        SetPieceAt(new Position(7, 1), new Knight(Color.Green, new Position(7, 1)));
        SetPieceAt(new Position(7, 2), new Bishop(Color.Green, new Position(7, 2)));
        SetPieceAt(new Position(7, 3), new Queen(Color.Green, new Position(7, 3)));
        SetPieceAt(new Position(7, 4), new King(Color.Green, new Position(7, 4)));
        SetPieceAt(new Position(7, 5), new Bishop(Color.Green, new Position(7, 5)));
        SetPieceAt(new Position(7, 6), new Knight(Color.Green, new Position(7, 6)));
        SetPieceAt(new Position(7, 7), new Rook(Color.Green, new Position(7, 7)));
    }

    // نمایش صفحه شطرنج در ترمینال
    public void PrintBoard()
    {
        bool bgWhite = true;

        int windowHeight = Console.WindowHeight;
        int y = (windowHeight - 13) / 2;
        string yString = new string('\n', y);

        int windowWidth = Console.WindowWidth;
        int textLength = 30;
        int padding = (windowWidth - textLength) / 2;
        string paddingString = new string(' ', padding);

        Console.WriteLine(yString + paddingString + "   A  B  C  D  E  F  G  H");

        for (int row = 7; row >= 0; row--)
        {
            Console.ResetColor();
            Console.Write(paddingString + (row + 1) + " ");

            for (int col = 0; col < 8; col++)
            {
                if (bgWhite)
                    Console.BackgroundColor = ConsoleColor.White;
                else
                    Console.BackgroundColor = ConsoleColor.Gray;

                Piece piece = GetPieceAt(new Position(row, col));

                if (piece != null && piece.Color == Color.Red)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.Write(piece == null ? "   " : $" {piece.Symbol} ");

                bgWhite = !bgWhite;
            }
            bgWhite = !bgWhite;

            Console.WriteLine();
        }

        Console.ResetColor();
        Console.WriteLine(yString);
    }
}
