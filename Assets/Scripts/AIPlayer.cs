//AI Player Class to implement the Ai player with alpha-beta prunning algorithm

using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
using System.Linq;
using System;

public class AIPlayer
{
    private int searchDepth { set; get; }
    private ObjectPool<GameState> gameStatePool;

    public AIPlayer(int depth)
    {
        this.searchDepth = depth;
        this.gameStatePool = new ObjectPool<GameState>(() => new GameState());
    }
    
    //---------------------All Heuristics Evaluation Function-----------------------

    private int CalculateHeuristics(GameState gameState)
    {
        int coinParityHeuristic = CoinParityHeuristic(gameState);
        int actualMobilityHeuristic = ActualMobilityHeuristic(gameState);
        int potentialMobilityHeuristic = PotentialMobilityHeuristic(gameState);
        int cornersHeuristic = CornersCapturedHeuristic(gameState);

        return (coinParityHeuristic + actualMobilityHeuristic + potentialMobilityHeuristic + cornersHeuristic);
        //return (coinParityHeuristic + actualMobilityHeuristic + cornersHeuristic);

    }   

    //---------------------All Heuristics Evaluation Function-----------------------

    private int CalculateHeuristics(GameState gameState)
    {
        int coinParityHeuristic = CoinParityHeuristic(gameState);
        int actualMobilityHeuristic = ActualMobilityHeuristic(gameState);
        int potentialMobilityHeuristic = PotentialMobilityHeuristic(gameState);
        int cornersHeuristic = CornersCapturedHeuristic(gameState);

        return (coinParityHeuristic + actualMobilityHeuristic + potentialMobilityHeuristic + cornersHeuristic);

    }


    //-------------------------Heuristics Calculations------------------------------

    // Heuristics to take in consideration
    // 1. Coin Parity
    // 2. Mobility
    // 3. Corners Captured

    //-----------------------------Coin Parity---------------------------------------

    private int CoinParityHeuristic(GameState gameState)
    {
        Player currentPLayer = gameState.CurrentPlayer.Opponent();

        // A simple evaluation function that counts the coin parity heuristic
        int blackScore = gameState.DiscCount[Player.Black];
        int whiteScore = gameState.DiscCount[Player.White];

        if ((blackScore + whiteScore) == 0)
        {
            return 0;
        }

        if (currentPLayer == Player.Black)
        {
            return (100 * (blackScore - whiteScore)) / (blackScore + whiteScore);
        }

        else if (currentPLayer == Player.White)
        {
            return (100 * (whiteScore - blackScore)) / (blackScore + whiteScore);
        }

        return 0;
    }
    //------------------------------Actual Mobility----------------------------------

    private int ActualMobilityHeuristic(GameState gameState)
    {
        // A simple evaluation function that calculates the actual mobility heuristic
        Player maxPlayer = gameState.CurrentPlayer;
        Player minPlayer = maxPlayer.Opponent();

        if (maxPlayer == Player.None)
        {
            return 0;
        }

        int maxMobility = gameState.FindLegalMoves(maxPlayer).Count;
        int minMobility = gameState.FindLegalMoves(minPlayer).Count;

        if ((maxMobility + minMobility) != 0)
        {
            return (100 * (maxMobility - minMobility)) / (maxMobility + minMobility);
        }

        else
        {
            return 0;
        }

    }
     //---------------------------------Potential Mobility-------------------------------

    private int PotentialMobilityHeuristic(GameState gameState)
    {
        // A simple evaluation function that calculates the potential mobility heuristic
        
        Player minPlayer = gameState.CurrentPlayer;
        Player maxPlayer = minPlayer.Opponent();

        if (maxPlayer == Player.None)
        {
            return 0;
        }

        int maxMobility = GetPotentialMobility(gameState, maxPlayer);
        int minMobility = GetPotentialMobility(gameState, minPlayer);

        if ((maxMobility + minMobility) != 0)
        {
            return (100 * (maxMobility - minMobility)) / (maxMobility + minMobility);
        }

        else
        {
            return 0;
        }
        

    }
    
    //----------------------------Corners Captured----------------------------------//

    private int CornersCapturedHeuristic(GameState gameState)
    {
        Player currentPLayer = gameState.CurrentPlayer;

        // A simple evaluation function that calculates the potential mobility heuristic

        int minScore = GetCornersCaptured(gameState, currentPLayer);
        int maxScore = GetCornersCaptured(gameState, currentPLayer.Opponent());
        int result = 0;

        if ((maxScore + minScore) != 0)
        {
            result = 100 * (maxScore - minScore) / (maxScore + minScore);
            return result;
        }
        else
        {
            result = 0;
            return result;
        }
    }


    private int GetCornersCaptured(GameState gameState, Player player)
    {
        int cornersCaptured = 0;

        // Define the positions of the four corners on the game board
        Position topLeftCorner = new Position(0, 0);
        Position topRightCorner = new Position(0, GameState.Cols - 1);
        Position bottomLeftCorner = new Position(GameState.Rows - 1, 0);
        Position bottomRightCorner = new Position(GameState.Rows - 1, GameState.Cols - 1);

        // Check each corner position to see if the player has a piece there
        if (gameState.Board[topLeftCorner.Row, topLeftCorner.Col] == player)
        {
            cornersCaptured++;
        }
        if (gameState.Board[topRightCorner.Row, topRightCorner.Col] == player)
        {
            cornersCaptured++;
        }
        if (gameState.Board[bottomLeftCorner.Row, bottomLeftCorner.Col] == player)
        {
            cornersCaptured++;
        }
        if (gameState.Board[bottomRightCorner.Row, bottomRightCorner.Col] == player)
        {
            cornersCaptured++;
        }

        return cornersCaptured;
    }


}
