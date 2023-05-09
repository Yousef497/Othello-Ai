using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    //------------UI References------------------------

    [SerializeField]
    private TextMeshProUGUI topText;

    [SerializeField]
    private TextMeshProUGUI blackScoreText;

    [SerializeField]
    private TextMeshProUGUI whiteScoreText;

    [SerializeField]
    private TextMeshProUGUI blackScoreAllGame;

    [SerializeField]
    private TextMeshProUGUI whiteScoreAllGame;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private Image blackOverlay;

    [SerializeField]
    private RectTransform playAgainButton;

    //--------Some GameObject variables to control the scene-------
    [SerializeField]
    private GameObject restartConfirmDialog;

    [SerializeField]
    private GameObject mainMenuConfirmDialog;

    [SerializeField]
    private GameObject restartContainer;

    [SerializeField]
    private GameObject disableClickPlane;


    //-------------Control Scene--------------------------------------

    public void SetPlayerText(Player currentPlayer)
    {
        if (currentPlayer == Player.Black)
        {
            topText.text = "Black's Turn <sprite name=DiscBlackUp>";
        }
        else if (currentPlayer == Player.White)
        {
            topText.text = "White's Turn <sprite name=DiscWhiteUp>";
        }
    }

    public void SetSkippedText(Player skippedPlayer)
    {
        if (skippedPlayer == Player.Black)
        {
            topText.text = "Black Cannot Move! <sprite name=DiscBlackUp>";
        }
        else if (skippedPlayer == Player.White)
        {
            topText.text = "White Cannot Move! <sprite name=DiscWhiteUp>";
        }
    }

    // helper method to implement the end of the game
    public void SetTopText(string message)
    {
        topText.text = message;
    }

    //-------------Animations------------------------------
    public IEnumerator AnimateTopText()
    {
        topText.transform.LeanScale(Vector3.one * 1.2f, 0.25f).setLoopPingPong(4);
        yield return new WaitForSeconds(2);
    }

    private IEnumerator ScaleDown(RectTransform rect)
    {
        rect.LeanScale(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.2f);
        rect.gameObject.SetActive(false);
    }

    private IEnumerator ScaleUp(RectTransform rect)
    {
        rect.gameObject.SetActive(true);
        rect.localScale = Vector3.zero;
        rect.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator ShowScoreText()
    {
        yield return ScaleDown(topText.rectTransform);
        yield return ScaleUp(blackScoreText.rectTransform);
        yield return ScaleUp(whiteScoreText.rectTransform);
        yield return ScaleDown(blackScoreAllGame.rectTransform);
        yield return ScaleDown(whiteScoreAllGame.rectTransform);
    }

    public void SetBlackScoreText(int score)
    {
        blackScoreText.text = $"<sprite name=DiscBlackUp> {score}";
    }

    public void SetBlackScoreAllText(int score)
    {
        blackScoreAllGame.text = $"<sprite name=DiscBlackUp> {score}";
        blackScoreAllGame.transform.LeanScale(Vector3.one * 1.2f, 0.2f).setLoopPingPong(1);
    }

    public void SetWhiteScoreText(int score)
    {
        whiteScoreText.text = $"<sprite name=DiscWhiteUp> {score}";
    }

    public void SetWhiteScoreAllText(int score)
    {
        whiteScoreAllGame.text = $"<sprite name=DiscWhiteUp> {score}";
        whiteScoreAllGame.transform.LeanScale(Vector3.one * 1.2f, 0.2f).setLoopPingPong(1);
    }

    private IEnumerator ShowOverlay()
    {
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.color = Color.clear;
        blackOverlay.rectTransform.LeanAlpha(0.8f, 1);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator HideOverlay()
    {
        blackOverlay.rectTransform.LeanAlpha(0, 1);
        yield return new WaitForSeconds(1);
        blackOverlay.gameObject.SetActive(false);
    }

    private IEnumerator MoveScoreDown()
    {
        blackScoreText.rectTransform.LeanMoveY(0, 0.5f);
        whiteScoreText.rectTransform.LeanMoveY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void SetWinnerText(Player winner)
    {
        switch (winner)
        {
            case Player.Black:
                winnerText.text = "Black Won!";
                break;

            case Player.White:
                winnerText.text = "White Won!";
                break;

            case Player.None:
                winnerText.text = "It's a Tie!";
                break;
        }
    }

    public IEnumerator ShowEndScreen()
    {
        yield return ShowOverlay();
        yield return MoveScoreDown();
        yield return ScaleUp(winnerText.rectTransform);
        yield return ScaleUp(playAgainButton);
    }

    public IEnumerator HideEndScreen()
    {
        StartCoroutine(ScaleDown(winnerText.rectTransform));
        StartCoroutine(ScaleDown(blackScoreText.rectTransform));
        StartCoroutine(ScaleDown(whiteScoreText.rectTransform));
        StartCoroutine(ScaleDown(playAgainButton));

        yield return new WaitForSeconds(0.5f);
        yield return HideOverlay();
    }


    //-----------------Restart and Main Menu--------------------------

    public void onRestartCilcked()
    {
        restartContainer.SetActive(false);
        restartConfirmDialog.SetActive(true);
        disableClickPlane.SetActive(true);
    }

    public void restartConfirmNo()
    {
        restartContainer.SetActive(true);
        restartConfirmDialog.SetActive(false);
        disableClickPlane.SetActive(false);
    }

    public void onMainMenuClicked()
    {
        restartContainer.SetActive(false);
        mainMenuConfirmDialog.SetActive(true);
        disableClickPlane.SetActive(true);
    }

    public void mainMenuConfirmNo()
    {
        restartContainer.SetActive(true);
        mainMenuConfirmDialog.SetActive(false);
        disableClickPlane.SetActive(false);
    }

    //-------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        mainMenuConfirmDialog.SetActive(false);
        restartConfirmDialog.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
