using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using System.Threading;
//using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public string MainMenu;
    private int gameMode;
    private int AiBlackLevel;
    private int AiWhiteLevel;


    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Disc discBlackUp;

    [SerializeField]
    private Disc discWhiteUp;

    [SerializeField]
    private GameObject highlightPrefab;

    [SerializeField]
    private UIManager uiManager;

    // ----------------------------------------------------------

    private Dictionary<Player, Disc> discPrefabs = new Dictionary<Player, Disc>();
    private GameState gameState = new GameState();
    private Disc[,] discs = new Disc[8, 8];
    private List<GameObject> highlights = new List<GameObject>();

    private AIPlayer AIBlack = new AIPlayer();
    private AIPlayer AIWhite = new AIPlayer();


    //------------------------------------------------------------


    // ----------------------------Functions and Methods--------------------------------




    //----------------------------PC VS PC--------------------------------

    private IEnumerator PCVSPC()
    {
        // The AI player will be black
        if (gameState.CurrentPlayer == Player.Black)
        {
            Position AIMove = AIBlack.GetBestMove(gameState);
            yield return new WaitForSeconds(2f);
            OnBoardClicked(AIMove);
        }

        else if (gameState.CurrentPlayer == Player.White)
        {
            Position AIMove = AIWhite.GetBestMove(gameState);
            yield return new WaitForSeconds(2f);
            OnBoardClicked(AIMove);
        }
    }


    //----------------------------Human VS PC-----------------------------

    private IEnumerator HumanVSPC()
    {
        // The AI player will be black
        if (gameState.CurrentPlayer == Player.Black)
        {
            Position AIMove = AIBlack.GetBestMove(gameState);
            yield return new WaitForSeconds(2f);
            OnBoardClicked(AIMove);
        }

        else if (gameState.CurrentPlayer == Player.White)
        {
            //yield return new WaitForSeconds(1f);
            HumanPlay();
            //yield return new WaitForSeconds(2f);
        }
    }


    //-----------------------------Human VS Human-------------------------

    private void HumanPlay()
    {
        // ----- Left Mouse button clicked -----
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (!IsPointerOverUIObject())
                {
                    Vector3 impact = hitInfo.point;
                    Position boardPos = SceneToBoardPos(impact);
                    OnBoardClicked(boardPos);
                }

            }
        }
    }


    // Disable clicks through UI Elements, not to click on highlights
    // when a pop up of restart or main menu confirmation messages
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    //--------------------Highlights Methods---------------------------
    private void ShowLegalMoves()
    {
        foreach (Position boardPos in gameState.LegalMoves.Keys)
        {
            Vector3 scenePos = BoardToScenePos(boardPos) + Vector3.up * 0.01f;
            GameObject highlight = Instantiate(highlightPrefab, scenePos, Quaternion.identity);
            highlights.Add(highlight);
        }
    }

    private void HideLegalMoves()
    {
        highlights.ForEach(Destroy);
        highlights.Clear();
    }

    
    private void OnBoardClicked(Position boardPos)
    {
        if (gameState.MakeMove(boardPos, out MoveInfo moveInfo))
        {
            StartCoroutine(OnMoveMade(moveInfo));
        }
    }


    // --------------------Some Helper Methods---------------------------

    private Position SceneToBoardPos(Vector3 scenePos)
    {
        int col = (int)(scenePos.x - 0.25f);
        int row = 7 - (int)(scenePos.z - 0.25f);
        return new Position(row, col);
    }

    private Vector3 BoardToScenePos(Position boardPos)
    {
        return new Vector3(boardPos.Col + 0.75f, 0, 7 - boardPos.Row + 0.75f);
    }

    private void SpawnDisc(Disc prefab, Position boardPos)
    {
        Vector3 scenePos = BoardToScenePos(boardPos) + Vector3.up * 0.1f;
        discs[boardPos.Row, boardPos.Col] = Instantiate(prefab, scenePos, Quaternion.identity);
        //Debug.Log(scenePos);
    }

    private void AddStartDiscs()
    {
        foreach (Position boardPos in gameState.OccupiedPositions())
        {
            Player player = gameState.Board[boardPos.Row, boardPos.Col];
            SpawnDisc(discPrefabs[player], boardPos);
        }

        //Debug.Log(gameState.OccupiedPositions());
    }

    private void FlipDiscs(List<Position> positions)
    {
        foreach (Position boardPos in positions)
        {
            discs[boardPos.Row, boardPos.Col].Flip();
        }
    }



    // ------------------- Update co-routines --------------------------

    private IEnumerator ShowMove(MoveInfo moveInfo)
    {
        //yield return new WaitForSeconds(0.53f);
        SpawnDisc(discPrefabs[moveInfo.Player], moveInfo.Position);
        yield return new WaitForSeconds(0.33f);
        FlipDiscs(moveInfo.Outflanked);
        yield return new WaitForSeconds(0.83f);
    }

    private IEnumerator OnMoveMade(MoveInfo moveInfo)
    {
        //yield return new WaitForSeconds(2f);
        HideLegalMoves();
        yield return ShowMove(moveInfo);
        yield return ShowTurnOutcome(moveInfo);
        ShowLegalMoves();
    }

    private IEnumerator ShowTurnSkipped(Player skippedPlayer)
    {
        uiManager.SetSkippedText(skippedPlayer);
        yield return uiManager.AnimateTopText();
    }

    private IEnumerator ShowGameOver(Player winner)
    {
        uiManager.SetTopText("Neither Player Can Move");
        yield return uiManager.AnimateTopText();

        yield return uiManager.ShowScoreText();
        yield return new WaitForSeconds(0.5f);

        yield return ShowCounting();

        uiManager.SetWinnerText(winner);
        yield return uiManager.ShowEndScreen();
    }

    private IEnumerator ShowTurnOutcome(MoveInfo moveInfo)
    {
        if (gameState.GameOver)
        {
            yield return ShowGameOver(gameState.Winner);
            yield break;
        }

        Player currentPlayer = gameState.CurrentPlayer;

        if (currentPlayer == moveInfo.Player)
        {
            yield return ShowTurnSkipped(currentPlayer.Opponent());
        }

        uiManager.SetPlayerText(currentPlayer);
        BlackCount();
        WhiteCount();

    }

    //------------------Disc Counting Game Over--------------
    private IEnumerator ShowCounting()
    {
        int black = 0, white = 0;

        foreach (Position pos in gameState.OccupiedPositions())
        {
            Player player = gameState.Board[pos.Row, pos.Col];

            if (player == Player.Black)
            {
                black++;
                uiManager.SetBlackScoreText(black);
            }
            else if (player == Player.White)
            {
                white++;
                uiManager.SetWhiteScoreText(white);
            }

            discs[pos.Row, pos.Col].Twitch();
            yield return new WaitForSeconds(0.05f);
        }
    }

    //--------------------Disc Counting---------------------
    private void BlackCount()
    {
        int black = gameState.DiscCount[Player.Black];
        uiManager.SetBlackScoreAllText(black);
    }

    private void WhiteCount()
    {
        int white = gameState.DiscCount[Player.White];
        uiManager.SetWhiteScoreAllText(white);
    }


    //--------Restart Game Play Again Button after End Game-------------------
    private IEnumerator RestartGame()
    {
        yield return uiManager.HideEndScreen();
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void OnPlayAgainClicked()
    {
        StartCoroutine(RestartGame());
    }

    //----------Restart Game by user in the middle of the game-------------------
    public void RestartConfirmYes()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    //--------------------Return to Main Menu to change game mode---------------
    public void MainMenuConfirmYes()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
