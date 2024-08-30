using UnityEngine;

public class SplashContentLoader : MonoBehaviour
{
    public bool complete = false;

    public void Start()
    {
        LoadContent();
    }

    private void LoadContent()
    {
        /////Preload content here. 

        /////Complete loading and allow main to be loaded. 
        complete = true;
    }

    public static SplashContentLoader GetLoader()
    {
        return new GameObject("--ContentLoader-- ").AddComponent<SplashContentLoader>();
    }
}