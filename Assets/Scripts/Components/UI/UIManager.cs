using Purrcifer.UI;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    [SerializeField] private UI_ImageFader _transitionFader;
    [SerializeField] private UI_DialogueManager _dialogueManager;
    [SerializeField] private UI_PlayerHealthBarController _playerHealthBar;
    [SerializeField] private UI_BossHealthBar _bossHealthBar;
    private bool _fadeOpComplete = false;

    public BossHealth bossHealth = null; 

    public static UIManager Instance => _instance;

    public static UI_DialogueManager DialogueManager => Instance._dialogueManager;

    public UI_PlayerHealthBarController PlayerHealthBar => _playerHealthBar;

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

    private void Update()
    {
        if (bossHealth != null)
        {
            //Do boss healthbar updates here. 
            _bossHealthBar.UpdateState(bossHealth.Health);
        }
    }

    private void OnDisable()
    {
        _transitionFader.fadeOpComplete -= Instance.FadeOperationComplete;
    }

    public static void SetDialogue(Dialogue dialogueData)
    {
        _instance._dialogueManager.StartDialogue(dialogueData);
    }

    public static void SetBossHealth(BossHealth bossHealth)
    {
        Instance.bossHealth = bossHealth;
        Instance._bossHealthBar.SetUp(bossHealth);
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
