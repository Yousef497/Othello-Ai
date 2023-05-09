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

}
