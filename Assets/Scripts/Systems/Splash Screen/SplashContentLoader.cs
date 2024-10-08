﻿using DataManager;
using System.Collections;
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

    private void LoadContent() => StartCoroutine(Preload());

    private IEnumerator Preload()
    {
        /////Preload content here. 
        DataCarrier.Generate(); //Load the game save data. 
        while(DataCarrier.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        PlayerInputSys.CreateInstance();

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
