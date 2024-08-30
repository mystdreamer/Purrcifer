using UnityEngine;

/// <summary>
/// Tool for preloading data on the splash screen. 
/// </summary>
public class SplashContentLoader : MonoBehaviour
{
    public bool _complete = false;

    public bool Complete => _complete;

    public void Start()
    {
        //Start the content loading. 
        LoadContent();
    }

    private void LoadContent()
    {
        /////Preload content here. 

        /////Complete loading and allow main to be loaded. 
        _complete = true;
    }

    /// <summary>
    /// Static builder function.
    /// </summary>
    public static SplashContentLoader GetLoader()
    {
        return new GameObject("--ContentLoader-- ").AddComponent<SplashContentLoader>();
    }
}