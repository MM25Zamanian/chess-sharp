public abstract class Piece(Color color, Position position)
{
    public Color Color { get; protected set; } = color;
    public Position Position { get; set; } = position;
    public bool HasMoved { get; set; } = false;

    public abstract char Symbol { get; } // نماد مهره برای نمایش روی صفحه
    public abstract List<Position> GetPossibleMoves(Board board); // گرفتن لیست حرکات ممکن

    // بررسی اینکه آیا یک موقعیت روی صفحه است
    protected bool IsOnBoard(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    // بررسی اینکه آیا حرکتی باعث می‌شود شاه در معرض کیش قرار بگیرد
    public bool MoveLeadsToCheck(Board board, Position to)
    {
        Board tempBoard = new(board); // ساخت یک صفحه موقت
        Position originalPosition = Position;
        Piece targetPiece = tempBoard.GetPieceAt(to);

        // انجام حرکت در صفحه موقت
        tempBoard.MovePieceInternal(originalPosition, to);

        // بررسی کیش بودن شاه خودی
        bool isCheck = tempBoard.IsKingInCheck(Color);

        // برگرداندن وضعیت صفحه موقت به حالت قبل
        tempBoard.SetPieceAt(originalPosition, this);
        tempBoard.SetPieceAt(to, targetPiece);

        return isCheck;
    }
}
