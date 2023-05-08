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

}
