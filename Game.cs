public class Game
{
    private readonly Board _board;
    private Color _currentPlayer;

    public Game()
    {
        _board = new Board();
        _currentPlayer = Color.Red;
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
            _board.PrintBoard();
            Console.WriteLine($"  ➤ Player's turn {_currentPlayer}");

            if (_board.IsKingInCheck(_currentPlayer))
            {
                Console.WriteLine("  ➤ Cult!");
            }

            Console.Write("  ➤ Enter your move (E.g: e2 to e4) ⟹  ");

            string? move = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(move))
            {
                continue;
            }

            try
            {
                string[] parts = move.ToLower().Split(" to ");
                if (parts.Length != 2)
                {
                    Console.WriteLine("  ➤ The move format is invalid.");
                    continue;
                }

                Position from = Position.FromString(parts[0].Trim().ToUpper());
                Position to = Position.FromString(parts[1].Trim().ToUpper());

                Piece piece = _board.GetPieceAt(from);
                if (piece == null)
                {
                    Console.WriteLine("  ➤ There is no bead at the origin.");
                    continue;
                }

                if (piece.Color != _currentPlayer)
                {
                    Console.WriteLine("  ➤ This piece does not belong to you.");
                    continue;
                }

                _board.MovePiece(from, to);

                if (_board.IsCheckmate(_currentPlayer == Color.Red ? Color.Green : Color.Red))
                {
                    Console.WriteLine("  ➤ Checkmate");
                    Console.WriteLine($"  ➤ Player {_currentPlayer} won!");
                    break;
                }

                _currentPlayer = _currentPlayer == Color.Red ? Color.Green : Color.Red;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"  ➤ Err: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  ➤ Unk Err: {e.Message}");
            }

            Console.WriteLine("  ➤ Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
