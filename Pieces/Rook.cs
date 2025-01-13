public class Rook(Color color, Position position) : Piece(color, position)
{
    public override char Symbol => Color == Color.Red ? '♜' : '♖';

    public override List<Position> GetPossibleMoves(Board board)
    {
        List<Position> possibleMoves = [];
        int[][] directions = [[0, 1], [0, -1], [1, 0], [-1, 0]];

        foreach (var dir in directions)
        {
            for (int i = 1; ; i++)
            {
                int newRow = Position.Row + dir[0] * i;
                int newCol = Position.Col + dir[1] * i;

                if (!IsOnBoard(newRow, newCol)) break;

                Position newPosition = new(newRow, newCol);
                Piece pieceAtNewPosition = board.GetPieceAt(newPosition);

                if (pieceAtNewPosition == null)
                {
                    possibleMoves.Add(newPosition);
                }
                else
                {
                    if (pieceAtNewPosition.Color != Color)
                    {
                        possibleMoves.Add(newPosition);
                    }
                    break;
                }
            }
        }

        return possibleMoves;
    }
}
