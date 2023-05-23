// This is a simple class to represent the position on the board

public class Position
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override bool Equals(object obj)
    {
        if (obj is Position other)
        {
            return Row == other.Row && Col == other.Col;
        }

        return false;
    }


    // this gives each cell on the board a unique hash code
    public override int GetHashCode()
    {
        return 8 * Row + Col;
    }

}
