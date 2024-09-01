﻿using Purrcifer.UI;
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
    public BossHealth bossHealth = null; 

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
        UpdateBossUI();
    }

    public void ResetUIState()
    {
        _bossHealthBar.Deactivate();
        _gameOverController.DeactivateGameOverScreen();

    }

    #region Gameplay Updating.

    private void UpdateBossUI()
    {
        if (bossHealth != null)
        {
            //Do boss healthbar updates here. 
            _bossHealthBar.UpdateState(bossHealth.Health);
        }
    }

    #endregion

    #region UI Management.
    public static void SetDialogue(Dialogue dialogueData)
    {
        _instance._dialogueManager.StartDialogue(dialogueData);
    }

    public static void SetBossHealth(BossHealth bossHealth)
    {
        Instance.bossHealth = bossHealth;
        Instance._bossHealthBar.SetUp(bossHealth);
    }

    public static void EnableGameOverScreen()
    {
        Instance._gameOverController.EnableGameOverScreen();
    }
    #endregion

    #region UI Transition Fade Functions. 

    public void StartLevelTransitionFade(LevelLoading.LevelID levelToLoad, bool fadeOnLoad)
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
