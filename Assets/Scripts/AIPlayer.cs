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
    //-----------------------Get best move to play---------------------------------

    public Position GetBestMove(GameState gameState)
    {
        // Get a GameState instance from the object pool to improve memory
        GameState pooledGameState = gameStatePool.GetObject();
        pooledGameState.CopyFrom(gameState);

        Position bestMove = MiniMax(pooledGameState, searchDepth).Position;

        // Return the GameState instance to the object pool
        gameStatePool.PutObject(pooledGameState);

        return bestMove;
    }
    //------------------------MiniMax Alpha-Beta Pruning---------------------------

    private (int Score, Position Position) MiniMax(GameState gameState, int depth, int alpha = int.MinValue, int beta = int.MaxValue, bool maximizingPlayer = true, int timeoutSeconds = 3)
    {
        if (depth == 0 || gameState.GameOver)
        {
            return (CalculateHeuristics(gameState), null);
        }

        List<Position> legalMoves = new List<Position>(gameState.LegalMoves.Keys);

        // if player is black, then try to maximize
        // else, try to minimize
        Func<int, int, int> Optimizer = maximizingPlayer ? (Func<int, int, int>)Math.Max : (Func<int, int, int>)Math.Min;

        int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;
        Position bestMove = null;

        // Set a timer for th AI player
        bool timeoutReached = false;
        var timer = new System.Timers.Timer(timeoutSeconds * 1000);
        timer.Elapsed += (sender, e) =>
        {
            timeoutReached = true;
            timer.Stop();
        };
        timer.Start();


        foreach (Position move in legalMoves)
        {
            if (timeoutReached)
            {
                break;
            }

            // Get a GameState instance from the object pool
            GameState pooledGameState = gameStatePool.GetObject();
            pooledGameState.CopyFrom(gameState);
            pooledGameState.MakeMove(move, out _);

            int score = MiniMax(pooledGameState, depth - 1, alpha, beta, !maximizingPlayer, timeoutSeconds).Score;

            if (bestScore != Optimizer(score, bestScore))
            {
                bestMove = move;
                bestScore = score;
            }

            if (maximizingPlayer)
            {
                alpha = Optimizer(alpha, bestScore);
            }

            else
            {
                beta = Optimizer(beta, bestScore);
            }

            if (beta <= alpha)
            {
                break;     // Alpha-Beta Cut off
            }

            // Return the GameState instance to the object pool
            gameStatePool.PutObject(pooledGameState);

        }

        timer.Stop();

        return (bestScore, bestMove);

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
