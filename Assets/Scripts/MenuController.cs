using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{

    //------------Scenes acoording to chosen settings-------------
    public string Human_VS_Human;
    //--------------------------------------------------------------

    public GameObject modeNotSelected;
    public GameObject Difficulty_Human_VS_PC;
    public GameObject Difficulty_PC_VS_PC;
    public GameObject menu;

    //Public Variables to use to choose the scene
    public int gameMode = 0;
    public int AiBlackLevel = 0;
    public int AiWhiteLevel = 0;

    // ---------------------------------------------------------------------
    void Start()
    {
        modeNotSelected.SetActive(false);
        Difficulty_Human_VS_PC.SetActive(false);
        Difficulty_PC_VS_PC.SetActive(false);
    }

    public void OnAiBlackLevelChanged(TMP_Dropdown AiBlackLevelList)
    {
        AiBlackLevel = AiBlackLevelList.value;

        if (AiBlackLevel == 1)
        {
            PlayerPrefs.SetInt("AiBlackLevel", AiBlackLevel);
        }
        else if (AiBlackLevel == 2)
        {
            PlayerPrefs.SetInt("AiBlackLevel", 3);
        }
        else
        {
            PlayerPrefs.SetInt("AiBlackLevel", 5);
        }
    }

    public void OnAiWhiteLevelChanged(TMP_Dropdown AiWhiteLevelList)
    {
        AiWhiteLevel = AiWhiteLevelList.value;

        if (AiWhiteLevel == 1)
        {
            PlayerPrefs.SetInt("AiWhiteLevel", AiWhiteLevel);
        }
        else if (AiWhiteLevel == 2)
        {
            PlayerPrefs.SetInt("AiWhiteLevel", 3);
        }
        else
        {
            PlayerPrefs.SetInt("AiWhiteLevel", 5);
        }
    }

    //public void OnDifficultyChanged(TMP_Dropdown difficultyList)
    //{
    //    difficulty = difficultyList.value;
    //    PlayerPrefs.SetInt("Difficulty", difficulty);
    //}

    public void onGameModeChoose(TMP_Dropdown gameModeList)
    {
        gameMode = gameModeList.value;
        PlayerPrefs.SetInt("Mode", gameMode);
    }

    public void StartGame()
    {
        if (gameMode == 0)
        {
            modeNotSelected.SetActive(true);
            menu.SetActive(false);
        }

        // ----------------------------Human VS Human-----------------------------
        else if (gameMode == 1)
        {
            SceneManager.LoadScene(Human_VS_Human);
            //Debug.Log("Human VS Human");
        }

        //------------------------------Human VS PC-------------------------------
        else if (gameMode == 2)
        {
            Difficulty_Human_VS_PC.SetActive(true);
            menu.SetActive(false);
        }

        //----------------------------------PC VS PC-------------------------------
        else
        {
            Difficulty_PC_VS_PC.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OnOkayHumanVsPcSet()
    {
        if (AiBlackLevel != 0)
        {
            SceneManager.LoadScene(Human_VS_Human);
            //Debug.Log("Working try to go the scene");
        }
    }

    public void OnOkayPcVsPcSet()
    {
        if (AiBlackLevel != 0 && AiWhiteLevel != 0)
        {
            SceneManager.LoadScene(Human_VS_Human);
            //Debug.Log("Working try to go the scene");
        }
    }

    //private void Update()
    //{
    //    Debug.Log("Mode: " + gameMode + " Black: " + AiBlackLevel + " White: " + AiWhiteLevel);
    //}

    // -------------------------------------------------------

    //           options menu to be implemented

    // -------------------------------------------------------
}
