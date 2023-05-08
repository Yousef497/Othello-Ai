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

}
