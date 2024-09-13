using Purrcifer.Window.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        DataCarrier.Generate(); //Load the game save data. 
        PlayerInputSys refSet = PlayerInputSys.Instance;
        /////Preload content here. 

        SceneManager.LoadSceneAsync("UI_", LoadSceneMode.Additive);

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

public class WindowStateHandler : MonoBehaviour
{
    private static WindowStateHandler _instance;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            DestroyImmediate(_instance);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GameWindowManagement.ManageWindowFullscreen();
    }

    public static void ChangeAspectRatio(int x, int y)
    {
        GameWindowManagement.ApplyAspectRatio(x, y);
    }

    public static void Generate()
    {
        new GameObject("----Window State Handler----").AddComponent<WindowStateHandler>();
    }
}