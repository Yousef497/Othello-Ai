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


    //------------------------------------------------------------
    
    


    // ----------------------------Functions and Methods--------------------------------

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
            // call routine to make a move on GUI (to be added later)
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
