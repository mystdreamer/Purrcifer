using System.Collections;
using UnityEngine;

/// <summary>
/// Controller required for menu functioning. 
/// </summary>
public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        //UIManager.FadeOut();
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.LEVEL_1, false);
    }

    /// <summary>
    /// Function used for loading a previous save state. 
    /// </summary>
    public void LoadGame()
    {
        UIManager.Instance.StartLevelTransitionFade(DataCarrier.SavedLevel, false);
    }

    /// <summary>
    /// Function required for opening the settings menu. 
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("Not implemented: Settings.");
    }

    /// <summary>
    /// Function required for loading the credits menu. 
    /// </summary>
    public void OpenCredits()
    {
        Debug.Log("Not implemented: Credits.");
    }

    /// <summary>
    /// Function used for exiting the application. 
    /// </summary>
    public void ExitApplication()
    {
        Application.Quit();
    }
}
