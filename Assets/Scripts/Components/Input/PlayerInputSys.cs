using JetBrains.Annotations;
using Purrcifer.Inputs.Container;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Component used for handling player input commands. 
/// </summary>
public class PlayerInputSys : MonoBehaviour
{
    private static PlayerInputSys instance;

    /// <summary>
    /// The current keycode data assigned in the editor. 
    /// </summary>
    public List<Player_InputKey> keycodeData;
    public List<PlayerInput_ControllerButton> buttonData;
    public List<PlayerInput_ControllerAxis> axisData;
    public PlayerInput[] inputs;

    /// <summary>
    /// Returns the current instance of InputSys. 
    /// </summary>
    public static PlayerInputSys Instance
    {
        get { 
            if(instance == null)
            {
                GameObject obj = new GameObject("-----Menu PIS-----");
                PlayerInputSys pis = obj.AddComponent<PlayerInputSys>();
                return pis;
            }
            return instance; 
        }
    }

    void Awake()
    {
        #region Singleton.
        ///Setup singleton. 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
        #endregion

        SetMenu();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnUnloaded;
        SceneManager.sceneLoaded += OnUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnUnloaded;
        SceneManager.sceneLoaded -= OnUnloaded;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        //Update input here. 
        UpdateInput();
    }

    public Player_InputKey GetKey(PlayerActionIdentifier type)
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            if (keycodeData[i].IsInput(type))
                return keycodeData[i];
        }
        return null;
    }

    public PlayerInput_ControllerButton GetButton(PlayerActionIdentifier type)
    {
        for (int i = 0; i < buttonData.Count; i++)
        {
            if (buttonData[i].IsInput(type))
                return buttonData[i];
        }
        return null;
    }

    public PlayerInput_ControllerAxis GetAxis(PlayerActionIdentifier type)
    {
        for (int i = 0; i < axisData.Count; i++)
        {
            if (axisData[i].IsInput(type))
                return axisData[i];
        }
        return null;
    }

    /// <summary>
    /// Update all inputs in the system. 
    /// </summary>
    private void UpdateInput()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i] != null)
                inputs[i].Command();
        }
    }

    private void SetMenu()
    {
        ///Setup key list. 
        keycodeData = new List<Player_InputKey>()
        {
            new Player_InputKey(KeyCode.W, PlayerActionIdentifier.M_UP, "Move Up"),
            new Player_InputKey(KeyCode.S, PlayerActionIdentifier.M_DOWN, "Move Down"),
            new Player_InputKey(KeyCode.Space, PlayerActionIdentifier.A_INTERACT, "Action Submit")
        };

        buttonData = new List<PlayerInput_ControllerButton>() {
            new PlayerInput_ControllerButton(KeyCode.Joystick1Button0, PlayerActionIdentifier.A_INTERACT, "Action Submit")
        };

        axisData = new List<PlayerInput_ControllerAxis>()
        {
            new PlayerInput_ControllerAxis(PlayerActionIdentifier.AXIS_LEFT_STICK, "AXIS_LEFT_STICK"),
            new PlayerInput_ControllerAxis(PlayerActionIdentifier.AXIS_DPAD, "AXIS_DPAD"),
        };

        Setup();
    }

    private void SetGameplay()
    {
        ///Setup key list. 
        keycodeData = new List<Player_InputKey>()
        {
            new Player_InputKey(KeyCode.W, PlayerActionIdentifier.M_UP, "MOVE_UP"),
            new Player_InputKey(KeyCode.S, PlayerActionIdentifier.M_DOWN, "MOVE_DOWN"),
            new Player_InputKey(KeyCode.A, PlayerActionIdentifier.M_LEFT, "MOVE_LEFT"),
            new Player_InputKey(KeyCode.D, PlayerActionIdentifier.M_RIGHT, "MOVE_RIGHT"),
            new Player_InputKey(KeyCode.UpArrow, PlayerActionIdentifier.ACTION_UP, "ACTION_UP"),
            new Player_InputKey(KeyCode.LeftArrow, PlayerActionIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new Player_InputKey(KeyCode.DownArrow, PlayerActionIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new Player_InputKey(KeyCode.RightArrow, PlayerActionIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        buttonData = new List<PlayerInput_ControllerButton>() {
            new PlayerInput_ControllerButton(KeyCode.Joystick1Button3, PlayerActionIdentifier.ACTION_UP, "ACTION_UP"),
            new PlayerInput_ControllerButton(KeyCode.Joystick1Button2, PlayerActionIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new PlayerInput_ControllerButton(KeyCode.Joystick1Button1, PlayerActionIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new PlayerInput_ControllerButton(KeyCode.Joystick1Button0, PlayerActionIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        axisData = new List<PlayerInput_ControllerAxis>()
        {
            new PlayerInput_ControllerAxis(PlayerActionIdentifier.AXIS_LEFT_STICK, "AXIS_LEFT_STICK"),
            new PlayerInput_ControllerAxis(PlayerActionIdentifier.AXIS_RIGHT_STICK, "AXIS_RIGHT_STICK"),
            new PlayerInput_ControllerAxis(PlayerActionIdentifier.AXIS_DPAD, "AXIS_DPAD"),
        };
        Setup();
    }

    private void Setup()
    {
        // Generate Array. 
        List<PlayerInput> list = new List<PlayerInput>();
        list.AddRange(keycodeData);
        list.AddRange(buttonData);
        list.AddRange(axisData);

        inputs = list.ToArray();
    }

    private void OnUnloaded(Scene current, LoadSceneMode mode)
    {
        if (current.buildIndex == (int)LevelLoading.LevelID.SPLASH ||
            current.buildIndex == (int)LevelLoading.LevelID.MAIN)
        {
            SetMenu();
        }
        else
        {
            SetGameplay(); 
        }
    }

    private void OnUnloaded(Scene current)
    {
        ClearDelegates();
    }

    private void ClearDelegates()
    {
        foreach (PlayerInput input in inputs)
        {
            input.Clear(); 
        }
    }
}
