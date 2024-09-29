using Purrcifer.Window.Management;
using UnityEngine;

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