using Purrcifer.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TransitionScreenHandler : MonoBehaviour
{
    [SerializeField] private UI_ImageFader _transitionFader;
    [SerializeField] private bool _fadeOnLoad = false;
    [SerializeField] private bool _fadeOpComplete = false;

    public bool FadeOnLoad
    {
        get => _fadeOnLoad;
        set => _fadeOnLoad = value;
    }

    public bool FadedIn
    {
        get => (_transitionFader.state == FadeState.IN);
    }

    public bool FadedOut
    {
        get => (_transitionFader.state == FadeState.OUT);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_fadeOnLoad)
        {
            Debug.Log(scene.name + ": Fade on load in-acted.");
            EndLevelTransition();
            _fadeOnLoad = false;
        }
        else
        {
            Debug.Log(scene.name + ": Fade on load not called.");
        }
    }

    public void StartLevelTransition(LevelLoading.LevelID levelToLoad, bool fadeOnLoad)
    {
        StartCoroutine(FadeLevelTransitionIn((int)levelToLoad, fadeOnLoad));
    }

    public void StartLevelTransition(int levelToLoad, bool fadeOnLoad)
    {
        StartCoroutine(FadeLevelTransitionIn(levelToLoad, fadeOnLoad));
    }

    public void EndLevelTransition() => StartCoroutine(FadeLevelTransitionOut());

    private IEnumerator FadeLevelTransitionIn(int levelToLoad, bool fadeOnLoad)
    {
        this._fadeOnLoad = fadeOnLoad;
        _transitionFader.FadeIn();
        while (_transitionFader.state != FadeState.IN)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);

        UIManager.Instance.ResetUIState();
        LevelLoading.LevelLoadHandler.LoadLevel(levelToLoad);
    }

    private IEnumerator FadeLevelTransitionOut()
    {
        _transitionFader.FadeOut();
        while (_transitionFader.state != FadeState.OUT)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
