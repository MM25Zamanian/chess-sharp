public class Pawn(Color color, Position position) : Piece(color, position)
{
    public override char Symbol => Color == Color.Red ? '♟' : '♙';

    public override List<Position> GetPossibleMoves(Board board)
    {
        List<Position> possibleMoves = [];
        int moveDirection = Color == Color.Red ? 1 : -1;
        int startRow = Color == Color.Red ? 1 : 6;

        // حرکت یک خانه به جلو
        int oneStepForwardRow = Position.Row + moveDirection;
        if (IsOnBoard(oneStepForwardRow, Position.Col) && board.GetPieceAt(new Position(oneStepForwardRow, Position.Col)) == null)
        {
            possibleMoves.Add(new Position(oneStepForwardRow, Position.Col));

            // حرکت دو خانه به جلو در اولین حرکت
            int twoStepsForwardRow = Position.Row + 2 * moveDirection;
            if (!HasMoved && IsOnBoard(twoStepsForwardRow, Position.Col) && board.GetPieceAt(new Position(twoStepsForwardRow, Position.Col)) == null)
            {
                possibleMoves.Add(new Position(twoStepsForwardRow, Position.Col));
            }
        }

        // گرفتن مهره به صورت مورب
        int[] captureCols = [-1, 1];
        foreach (int colOffset in captureCols)
        {
            int captureCol = Position.Col + colOffset;
            if (IsOnBoard(oneStepForwardRow, captureCol))
            {
                Piece targetPiece = board.GetPieceAt(new Position(oneStepForwardRow, captureCol));
                if (targetPiece != null && targetPiece.Color != Color)
                {
                    possibleMoves.Add(new Position(oneStepForwardRow, captureCol));
                }
            }
        }

        return possibleMoves;
    }
}
