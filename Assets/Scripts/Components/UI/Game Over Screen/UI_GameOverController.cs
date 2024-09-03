using Purrcifer.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;
using UnityEngine.InputSystem;

public class UI_GameOverController : MonoBehaviour
{
    public MenuIndexer menuIndexer;

    public GameObject gameOverRoot;
    public UI_ImageFader imageFader;
    public UI_TextFader textFader;
    public TextMeshProUGUI[] textElements;
    public Color inactiveColor;
    public Color activeColor;
    bool canUpdate = true;
    bool opActive = false;

    public void Start()
    {
        menuIndexer = new MenuIndexer(0, 0, 1, textElements, inactiveColor, activeColor);
        gameOverRoot.SetActive(false);
    }

    public void OnEnable()
    {
        
        RegisterInputs();
    }

    private void RegisterInputs()
    {
        PlayerInputSys inputSysRef = PlayerInputSys.Instance;
        inputSysRef.GetAxis(PlayerActionIdentifier.AXIS_LEFT_STICK).DoAction += ResolveVector;
        inputSysRef.GetAxis(PlayerActionIdentifier.AXIS_DPAD).DoAction += ResolveVector;
        inputSysRef.GetKey(PlayerActionIdentifier.M_UP).DoAction += UpKey;
        inputSysRef.GetKey(PlayerActionIdentifier.M_DOWN).DoAction += DownKey;
        inputSysRef.GetKey(PlayerActionIdentifier.ACTION_DOWN).DoAction += Accept;
        inputSysRef.GetButton(PlayerActionIdentifier.ACTION_DOWN).DoAction += Accept;
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
            switch (menuIndexer.CurrentIndex)
            {
                case 0:
                    NewGame();
                    break;
                case 1:
                    LoadMenu();
                    break;
            }
            opActive = true;
        }
    }

    public void EnableGameOverScreen()
    {
        gameOverRoot.SetActive(true);
        RegisterInputs();
    }

    public void DeactivateGameOverScreen()
    {
        gameOverRoot.SetActive(false);
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        Debug.Log("New Game Called");
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.LEVEL_1, false);
    }

    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void LoadMenu()
    {
        Debug.Log("Load Menu Called");
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.MAIN, true);
    }

    private IEnumerator MenuCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canUpdate = true;
    }
}
