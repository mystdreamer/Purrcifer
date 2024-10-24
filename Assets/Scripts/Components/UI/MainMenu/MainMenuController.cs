using Purrcifer.Data.Defaults;
using Purrcifer.Data.Player;
using Purrcifer.Inputs.Container;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MenuIndexer
{
    [SerializeField] private int _currentIndex;
    [SerializeField] private int _minIndex;
    [SerializeField] private int _maxIndex;
    private TextMeshProUGUI[] _textElements;
    Color _inactiveColor;
    Color _activeColor;

    public MenuIndexer(int currentIndex, int minIndex, int maxIndex, TextMeshProUGUI[] textElements, Color inactiveColor, Color activeColor)
    {
        this._currentIndex = currentIndex;
        this._minIndex = minIndex;
        this._maxIndex = maxIndex;
        this._textElements = textElements;
        this._inactiveColor = inactiveColor;
        this._activeColor = activeColor;
        UpdateIndex();
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            UpdateIndex();
        }
    }

    private void UpdateIndex()
    {
        if (_currentIndex < _minIndex)
            _currentIndex = _maxIndex - 1;
        if (_currentIndex > _maxIndex - 1)
            _currentIndex = _minIndex;

        for (int i = 0; i < _textElements.Length; i++)
            if (_textElements[i] != null)
                _textElements[i].color = (i == _currentIndex) ? _activeColor : _inactiveColor;
    }
}

public abstract class MenuBase : MonoBehaviour
{
    public MenuIndexer menuIndexer;
    public TextMeshProUGUI[] textElements;
    public Color inactiveColor;
    public Color activeColor;
    [SerializeField] bool canUpdate = true;
    [SerializeField] bool opActive = false;
    [SerializeField] private PlayerInputs _playerInputs;
    private PlayInput_ControllerAxis inputAxisResolver = new PlayInput_ControllerAxis();

    public int SetIndex
    {
        set => menuIndexer.CurrentIndex = value;
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        menuIndexer = new MenuIndexer(0, 0, textElements.Length, textElements, inactiveColor, activeColor);
        _playerInputs = GameManager.PlayerInputs;
        OnEnableOwner();
    }

    private void OnEnable()
    {
        menuIndexer = new MenuIndexer(0, 0, textElements.Length, textElements, inactiveColor, activeColor);
        OnEnableOwner();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void ResetState()
    {
        OnDisableOwner();
        canUpdate = true;
        opActive = false;
    }

    private void OnDisable() => ResetState();

    private void OnDestroy() => ResetState();

    private void ChangedActiveScene(Scene current, Scene next)=> ResetState();

    private IEnumerator MenuCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canUpdate = true;
    }

    #region Input. 

    private void UpdateInputs()
    {
        ResolveVector(inputAxisResolver.Command(_playerInputs.axis_m_left));

        if (Input.GetKey(_playerInputs.key_m_up))
            UpKey(true);

        if (Input.GetKey(_playerInputs.key_m_down))
            DownKey(true);

        if (Input.GetKey(GameManager.PlayerInputs.key_menu_a) | Input.GetKey(_playerInputs.ctlr_a))
            Accept(true);
    }

    private void ResolveVector(Vector3 result)
    {
        if (result.z < 0)
            DownKey(true);

        if (result.z > 0)
            UpKey(true);
    }

    private void UpKey(bool result)
    {
        if (canUpdate && result)
        {
            canUpdate = false;
            menuIndexer.CurrentIndex -= 1;
            StartCoroutine(MenuCooldown());
        }
    }

    private void DownKey(bool result)
    {
        if (canUpdate && result)
        {
            canUpdate = false;
            menuIndexer.CurrentIndex += 1;
            StartCoroutine(MenuCooldown());
        }
    }

    private void Accept(bool result)
    {
        if (result && !opActive)
        {
            opActive = true;
            switch (menuIndexer.CurrentIndex)
            {
                case 0:
                    OptionA();
                    break;
                case 1:
                    OptionB();
                    break;
                case 2:
                    OptionC();
                    break;
                case 3:
                    OptionD();
                    break;
                case 4:
                    OptionE();
                    break;
            }

        }
    }
    #endregion

    #region Virtual Functions. 
    internal virtual void OptionA() { }
    internal virtual void OptionB() { }
    internal virtual void OptionC() { }
    internal virtual void OptionD() { }
    internal virtual void OptionE() { }
    internal virtual void OnEnableOwner() { }
    internal virtual void OnDisableOwner() { }
    #endregion
}

/// <summary>
/// Controller required for menu functioning. 
/// </summary>
public class MainMenuController : MenuBase
{
    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        GameManager.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(Purrcifer.LevelLoading.LevelID.CHARACTER_SELECT, false);
    }

    /// <summary>
    /// Function used for loading a previous save state. 
    /// </summary>
    public void LoadGame()
    {
        Debug.LogError(">> Menu: Loading Game. ");
        UIManager.Instance.StartLevelTransitionFade(GameManager.GetSavedLevel, false);
    }

    /// <summary>
    /// Function required for opening the settings menu. 
    /// </summary>
    public void OpenSettings()
    {
        Debug.LogError(">> Menu: Opening Settings. ");
    }

    /// <summary>
    /// Function required for loading the credits menu. 
    /// </summary>
    public void OpenCredits()
    {
        Debug.LogError(">> Menu: Opening Credits. ");

    }

    /// <summary>
    /// Function used for exiting the application. 
    /// </summary>
    public void ExitApplication()
    {
        Debug.LogError(">> Menu: Exiting Application. ");
        Application.Quit();
    }

    internal override void OptionA() => NewGame();

    internal override void OptionB() => LoadGame();

    internal override void OptionC() => OpenSettings();

    internal override void OptionD() => OpenCredits();

    internal override void OptionE() => ExitApplication();
}

