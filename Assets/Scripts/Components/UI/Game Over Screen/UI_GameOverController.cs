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
        GameManager.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelID.LEVEL_1, false);
        this.enabled = false;
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    private void LoadMenu()
    {
        Debug.Log("Load Menu Called");
        GameManager.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelID.MAIN, true);
        this.enabled = false;
    }

    internal override void OptionA() => NewGame();

    internal override void OptionB() => LoadMenu();
    #endregion
}
