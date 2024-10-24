using Purrcifer.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;
using UnityEngine.InputSystem;
using Purrcifer.LevelLoading;

public class UI_GameOverController : MenuBase
{
    public GameObject gameOverRoot;
    public UI_ImageFader imageFader;
    public UI_TextFader textFader;

    internal override void OnEnableOwner()
    {
        gameOverRoot.SetActive(true);
        base.OnEnableOwner();
    }

    internal override void OnDisableOwner()
    {
        if (gameOverRoot != null)
            gameOverRoot.SetActive(false);
        base.OnDisableOwner();
    }

    #region Menu Actions. 
    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    private void NewGame()
    {
        Debug.Log("New Game Called");
        GameManager.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelID.CHARACTER_SELECT, false);
        this.enabled = false;
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    private void LoadMenu()
    {
        Debug.Log("Load Menu Called");
        GameManager.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelID.MENU, true);
        this.enabled = false;
    }

    internal override void OptionA() => NewGame();

    internal override void OptionB() => LoadMenu();
    #endregion
}
