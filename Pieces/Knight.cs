public class Knight(Color color, Position position) : Piece(color, position)
{
    public override char Symbol => Color == Color.Red ? '♞' : '♘';

    public override List<Position> GetPossibleMoves(Board board)
    {
        List<Position> possibleMoves = [];
        int[][] moves = [
                [-2, -1], [-2, 1],
                [-1, -2], [-1, 2],
                [1, -2], [1, 2],
                [2, -1], [2, 1]
            ];

        foreach (var move in moves)
        {
            int newRow = Position.Row + move[0];
            int newCol = Position.Col + move[1];

            if (IsOnBoard(newRow, newCol))
            {
                Position newPosition = new(newRow, newCol);
                Piece pieceAtNewPosition = board.GetPieceAt(newPosition);
                if (pieceAtNewPosition == null || pieceAtNewPosition.Color != Color)
                {
                    possibleMoves.Add(newPosition);
                }
            }
        }

        return possibleMoves;
    }
}
