using System.Collections;
using UnityEngine;

/// <summary>
/// Handles loading the main menu from the splash screen. 
/// </summary>
public class SplashHandler : MonoBehaviour
{
    [Header("Time till the main menu is loaded.")]
    public float waitTime = 0;
    private SplashContentLoader loader;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        Application.targetFrameRate = 60;
        Debug.developerConsoleEnabled = true;

        //Cache loader reference for completion checking.
        loader = SplashContentLoader.GetLoader();

        //Start the timer. 
        StartCoroutine(LoadMain());
    }

    public IEnumerator LoadMain()
    {
        //Wait the inital time required. 
        yield return new WaitForSeconds(waitTime);

        //If loading hasn't completed, 
        while (!loader.Complete)
        {
            //wait till the next frame is executed. 
            yield return new WaitForEndOfFrame();
        }

        //Load the main menu.
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.MAIN, true);
    }
}
