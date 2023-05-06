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
    private TextMeshProUGUI blackScoreAllGame;

    [SerializeField]
    private TextMeshProUGUI whiteScoreAllGame;

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

    public void SetBlackScoreAllText(int score)
    {
        blackScoreAllGame.text = $"<sprite name=DiscBlackUp> {score}";
        blackScoreAllGame.transform.LeanScale(Vector3.one * 1.2f, 0.2f).setLoopPingPong(1);
    }

    public void SetWhiteScoreAllText(int score)
    {
        whiteScoreAllGame.text = $"<sprite name=DiscWhiteUp> {score}";
        whiteScoreAllGame.transform.LeanScale(Vector3.one * 1.2f, 0.2f).setLoopPingPong(1);
    }

}
