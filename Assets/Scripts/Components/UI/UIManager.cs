using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour, IFadeCallback
{
    private static UIManager instance;
    private bool fadeOpComplete = false;
    public UIFader transitionFader;

    public static UIManager Instance => instance;

    public bool FadeOpComplete
    {
        get
        {
            if (fadeOpComplete)
            {
                fadeOpComplete = false;
                return true;
            }
            return fadeOpComplete;
        }
    }

    void IFadeCallback.FadeOpComplete()
    {
        fadeOpComplete = true;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);
    }

    public static void FadeIn()
    {
        instance.transitionFader.FadeIn(instance);
    }

    public static void FadeOut()
    {
        instance.transitionFader.FadeOut(instance);
    }
}