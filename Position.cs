public struct Position(int row, int col)
{
    public int Row { get; set; } = row;
    public int Col { get; set; } = col;

    public override string ToString()
    {
        return $"{(char)('A' + Col)}{Row + 1}";
    }

    public static Position FromString(string pos)
    {
        if (pos.Length != 2)
        {
            throw new ArgumentException("Invalid position format.");
        }

        int col = pos[0] - 'A';
        if (col < 0 || col > 7)
        {
            throw new ArgumentException("Invalid column.");
        }

        if (!int.TryParse(pos[1].ToString(), out int row) || row < 1 || row > 8)
        {
            throw new ArgumentException("Invalid row.");
        }

        return new Position(row - 1, col);
    }
}
