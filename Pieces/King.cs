public class King(Color color, Position position) : Piece(color, position)
{
    public override char Symbol => Color == Color.Red ? '♚' : '♔';

    public override List<Position> GetPossibleMoves(Board board)
    {
        List<Position> possibleMoves = [];

        int[][] moves = [
                [-1, -1], [-1, 0], [-1, 1],
                [0, -1],[0, 1],
                [1, -1],[1, 0],  [1, 1]
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
