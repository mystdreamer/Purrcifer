using System.Collections;
using UnityEngine;

/// <summary>
/// Controller required for menu functioning. 
/// </summary>
public class MenuController : MonoBehaviour
{   
    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        DataCarrier.Instance.ResetPlayerData();
        StartCoroutine(FadeTransitionIn((int)LevelLoading.LevelID.LEVEL_1));
    }

    /// <summary>
    /// Function used for loading a previous save state. 
    /// </summary>
    public void LoadGame()
    {
        StartCoroutine(FadeTransitionIn(DataCarrier.SavedLevel));
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

    /// <summary>
    /// Fade transition screen in and load the provided level. 
    /// </summary>
    IEnumerator FadeTransitionIn(int levelToLoad)
    {
        UIManager.FadeIn();

        while (!UIManager.Instance.FadeOpComplete)
        {
            yield return new WaitForEndOfFrame();
        }
        LevelLoading.LevelLoadHandler.LoadLevel(LevelLoading.LevelID.LEVEL_1);
    }
}
