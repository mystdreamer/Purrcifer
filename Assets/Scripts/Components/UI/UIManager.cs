using Purrcifer.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    [SerializeField] private UIImageFader _transitionFader;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private PlayerHealthBarController _playerHealthBar;
    private bool _fadeOpComplete = false;

    public static UIManager Instance => _instance;

    public static DialogueManager DialogueManager => Instance._dialogueManager;

    public PlayerHealthBarController PlayerHealthBar => _playerHealthBar;

    public bool FadeOpComplete
    {
        get
        {
            if (_fadeOpComplete)
            {
                _fadeOpComplete = false;
                return true;
            }
            return _fadeOpComplete;
        }
    }

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);
    }

    private void OnDisable()
    {
        _transitionFader.fadeOpComplete -= Instance.FadeOperationComplete;
    }

    public static void SetDialogue(Dialogue dialogueData)
    {
        _instance._dialogueManager.StartDialogue(dialogueData);
    }

    #region UI Fade Functions. 
    internal void FadeOperationComplete()
    {
        _fadeOpComplete = true;
        _transitionFader.fadeOpComplete -= Instance.FadeOperationComplete;
    }

    public static void FadeIn()
    {
        _instance._transitionFader.fadeOpComplete += Instance.FadeOperationComplete;
        _instance._transitionFader.FadeIn();
    }

    public static void FadeOut()
    {
        _instance._transitionFader.fadeOpComplete += Instance.FadeOperationComplete;
        _instance._transitionFader.FadeOut();
    }
    #endregion
}
