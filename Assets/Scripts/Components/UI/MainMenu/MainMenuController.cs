using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct MenuIndexer
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int minIndex;
    [SerializeField] private int maxIndex;

    public MenuIndexer(int currentIndex, int minIndex, int maxIndex)
    {
        this.currentIndex = currentIndex;
        this.minIndex = minIndex;
        this.maxIndex = maxIndex;
    }

    public int CurrentIndex
    {
        get => currentIndex;
        set
        {
            currentIndex = value;
            if (currentIndex < minIndex)
                currentIndex = maxIndex;
            if (currentIndex > maxIndex)
                currentIndex = minIndex;
        }
    }
}

/// <summary>
/// Controller required for menu functioning. 
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public PlayerInputSys inputSys;
    public MenuIndexer menuIndexer;
    bool canUpdate = true;

    private void Start()
    {
        menuIndexer = new MenuIndexer(0, 0, 4);
        RegisterInputs();
    }

    private void RegisterInputs()
    {
        inputSys.GetAxis(PlayerActionIdentifier.AXIS_LEFTSTICK).DoAction += ResolveVector;
        inputSys.GetAxis(PlayerActionIdentifier.AXIS_DPAD).DoAction += ResolveVector;
        inputSys.GetKey(PlayerActionIdentifier.M_UP).DoAction += UpKey;
        inputSys.GetKey(PlayerActionIdentifier.M_DOWN).DoAction += DownKey;
        inputSys.GetKey(PlayerActionIdentifier.A_INTERACT).DoAction += Accept;
        inputSys.GetButton(PlayerActionIdentifier.A_INTERACT).DoAction += Accept;
    }

    private void DeregisterInputs()
    {
        inputSys.GetAxis(PlayerActionIdentifier.AXIS_LEFTSTICK).DoAction -= ResolveVector;
        inputSys.GetAxis(PlayerActionIdentifier.AXIS_DPAD).DoAction -= ResolveVector;
        inputSys.GetKey(PlayerActionIdentifier.M_UP).DoAction -= UpKey;
        inputSys.GetKey(PlayerActionIdentifier.M_DOWN).DoAction -= DownKey;
        inputSys.GetKey(PlayerActionIdentifier.M_UP).DoAction -= Accept;
        inputSys.GetButton(PlayerActionIdentifier.A_INTERACT).DoAction -= Accept;
        DestroyImmediate(inputSys);
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
            menuIndexer.CurrentIndex += 1;
            StartCoroutine(MenuCooldown());
        }
    }

    private void DownKey(bool result)
    {
        if (canUpdate && result)
        {
            canUpdate = false;
            menuIndexer.CurrentIndex -= 1;
            StartCoroutine(MenuCooldown());
        }
    }

    private void Accept(bool result)
    {
        if (result)
        {
            switch (menuIndexer.CurrentIndex)
            {
                case 0:
                    NewGame();
                    break;
                case 1:
                    LoadGame();
                    break;
                case 2:
                    OpenSettings();
                    break;
                case 3:
                    OpenCredits();
                    break;
                case 4:
                    ExitApplication();
                    break;
            }
        }
    }


    /// <summary>
    /// Function used for loading a new game. 
    /// </summary>
    public void NewGame()
    {
        DeregisterInputs();
        DataCarrier.Instance.ResetPlayerData();
        UIManager.Instance.StartLevelTransitionFade(LevelLoading.LevelID.LEVEL_1, false);
    }

    /// <summary>
    /// Function used for loading a previous save state. 
    /// </summary>
    public void LoadGame()
    {
        DeregisterInputs();
        UIManager.Instance.StartLevelTransitionFade(DataCarrier.SavedLevel, false);
    }

    /// <summary>
    /// Function required for opening the settings menu. 
    /// </summary>
    public void OpenSettings()
    {
        DeregisterInputs();
        Debug.Log("Not implemented: Settings.");
    }

    /// <summary>
    /// Function required for loading the credits menu. 
    /// </summary>
    public void OpenCredits()
    {
        DeregisterInputs();
        Debug.Log("Not implemented: Credits.");
    }

    /// <summary>
    /// Function used for exiting the application. 
    /// </summary>
    public void ExitApplication()
    {
        DeregisterInputs();
        Application.Quit();
    }

    private IEnumerator MenuCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canUpdate = true;
    }
}

