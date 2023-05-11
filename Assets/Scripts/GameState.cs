using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


// Game Rules class

public class GameState
{
    public const int Rows = 8;
    public const int Cols = 8;

    // store what each cell contains
    public Player[,] Board { get; private set; }

    // this dictionary to keep track of black and white discs
    // key is player and value is the number of discs with the players color
    // facing up on the board
    // can be used to get the scores and display it on the screen
    public Dictionary<Player, int> DiscCount { get; private set; }

    // PLayer Turn
    public Player CurrentPlayer { get; private set; }

    // GameOver or not
    public bool GameOver { get; private set; }

    // Who the winner is
    public Player Winner { get; private set; }


    // which moves the current player can make and current state
    // key is the position of the move and the value is the list of all 
    // outflanked discs this will make it easier to check if the move is legal
    // and if so which discs should be flipped
    public Dictionary<Position, List<Position>> LegalMoves { get; private set; }





    //---------------------------------------------------Class Constructor--------------------------------
    // Class constructor
    // Assume the top left corner is postion (0, 0)
    // At start of the game 4 discs are at the middle
    // 2 blacks and 2 whites
    public GameState()
    {
        //initialize board
        Board = new Player[Rows, Cols];
        Board[3, 3] = Player.White;
        Board[3, 4] = Player.Black;
        Board[4, 3] = Player.Black;
        Board[4, 4] = Player.White;

        // initialize disc counts
        DiscCount = new Dictionary<Player, int>()
        {
            {Player.Black, 2 },
            {Player.White, 2 },
        };

        // Black starts the game
        CurrentPlayer = Player.Black;
        LegalMoves = FindLegalMoves(CurrentPlayer);
    }

    
    //---------------------------------------------------------------------
    // to get legal moves to position the disc
    // the position should be:
    //  1. Empty
    //  2. at least one disc of opponents discs outflanked

    private bool IsInsideBoard(int r, int c)
    {
        return r >= 0 && r < Rows && c >= 0 && c < Cols;
    }

    // method of checking and storing the positions that will be outflnaked or not
    private List<Position> OutflankedInDir(Position pos, Player player, int rDelta, int cDelta)
    {
        List<Position> outflanked = new List<Position>();
        int r = pos.Row + rDelta;
        int c = pos.Col + cDelta;

        while (IsInsideBoard(r,c) && Board[r,c] != Player.None)
        {
            if (Board[r,c] == player.Opponent())
            {
                outflanked.Add(new Position(r, c));
                r += rDelta;
                c += cDelta;
            }

            else if (Board[r,c] == player)
            {
                return outflanked;
            }
        }

        return new List<Position>();
    }


    // find outflanked discs in all directions
    private List<Position> Outflanked(Position pos, Player player)
    {
        List<Position> outflanked = new List<Position>();

        for (int rDelta = -1; rDelta <= 1; rDelta++)
        {
            for (int cDelta = -1; cDelta <= 1; cDelta++)
            {
                if (rDelta == 0 && cDelta == 0)
                {
                    continue;
                }

                outflanked.AddRange(OutflankedInDir(pos, player, rDelta, cDelta));
            }
        }

        return outflanked;

    }


    private bool IsMoveLegal(Player player, Position pos, out List<Position> outflanked)
    {
        if (Board[pos.Row, pos.Col] != Player.None)
        {
            outflanked = null;
            return false;
        }

        outflanked = Outflanked(pos, player);
        return outflanked.Count > 0;
    }


    public Dictionary<Position, List<Position>> FindLegalMoves(Player player)
    {
        Dictionary<Position, List<Position>> legalMoves = new Dictionary<Position, List<Position>>();

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                Position pos = new Position(r, c);

                if (IsMoveLegal(player, pos, out List<Position> outflanked))
                {
                    legalMoves[pos] = outflanked;
                }
            }
        }

        //Debug.Log(legalMoves.Count);
        return legalMoves;
    }

}
