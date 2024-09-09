using Purrcifer.LevelLoading;
using Purrcifer.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private GameObject _transitionCanvas;
    [SerializeField] private UI_DialogueManager _dialogueManager;
    [SerializeField] private UI_PlayerHealthBarController _playerHealthBar;
    [SerializeField] private UI_BossHealthBar _bossHealthBar;
    [SerializeField] private UI_GameOverController _gameOverController;
    [SerializeField] private UI_TransitionScreenHandler _transitionScreenHandler;

    #region Properties. 
    public Boss boss = null; 

    public static UIManager Instance => _instance;

    public static UI_DialogueManager DialogueManager => Instance._dialogueManager;

    public UI_PlayerHealthBarController PlayerHealthBar => _playerHealthBar;

    public bool TransitionActive => _transitionScreenHandler.FadedIn;

    public bool TransitionInactive => _transitionScreenHandler.FadedOut;

    #endregion

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_gameOverController.gameOverRoot);
            DontDestroyOnLoad(_transitionCanvas);
        }
        else
        {
            DestroyImmediate(gameObject);
            DestroyImmediate(_gameOverController.gameOverRoot);
            DestroyImmediate(_transitionCanvas);
        }
    }

    private void Update()
    {

    }

    public void ResetUIState()
    {
        _bossHealthBar.Deactivate();
        _gameOverController.enabled = false;
    }

    #region UI Management.
    public static void SetDialogue(Dialogue dialogueData)
    {
        _instance._dialogueManager.StartDialogue(dialogueData);
    }

    public static void SetBossHealth(Boss boss)
    {
        Instance.boss = boss;
        Instance._bossHealthBar.SetUp(boss);
    }

    public static void EnableGameOverScreen() => Instance._gameOverController.enabled = true;
    #endregion

    #region UI Transition Fade Functions. 

    public void StartLevelTransitionFade(LevelID levelToLoad, bool fadeOnLoad)
    {
        StartLevelTransitionFade((int)levelToLoad, fadeOnLoad);
    }

    public void StartLevelTransitionFade(int levelToLoad, bool fadeOnLoad)
    {
        _transitionScreenHandler.StartLevelTransition(levelToLoad, fadeOnLoad);
    }

    public void FadeLevelTransitionOut()
    {
        _transitionScreenHandler.EndLevelTransition();
    }
    #endregion
}
