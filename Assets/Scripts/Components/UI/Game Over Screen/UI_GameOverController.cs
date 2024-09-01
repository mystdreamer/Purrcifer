using Purrcifer.UI;
using System.Collections;
using UnityEngine;

public class UI_GameOverController : MonoBehaviour
{
    public GameObject gameOverRoot;
    public UI_ImageFader imageFader;
    public UI_TextFader textFader;

    public void Start()
    {
        gameOverRoot.SetActive(false);
    }

    public void EnableGameOverScreen()
    {
        gameOverRoot.SetActive(true);
    }

    public void DeactivateGameOverScreen()
    {
        gameOverRoot.SetActive(false);
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        Debug.Log("New Game Called");
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.LEVEL_1, false);
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void LoadMenu()
    {
        Debug.Log("Load Menu Called");
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.MAIN, true);
    }
}
