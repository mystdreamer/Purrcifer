using JetBrains.Annotations;
using Purrcifer.Data.Defaults;
using Purrcifer.Data.Player;
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
    public List<PInput_Key> keycodeData = new List<PInput_Key>();
    public List<PInput_ControllerButton> buttonData = new List<PInput_ControllerButton>();
    public List<PInput_ControllerAxis> axisData = new List<PInput_ControllerAxis>();
    public PlayerInput[] inputs;

    /// <summary>
    /// Returns the current instance of InputSys. 
    /// </summary>
    public static PlayerInputSys Instance
    {
        get => instance;
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

        SetInputs();
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

    void Update()
    {
        //Update input here. 
        UpdateInput();
    }

    public PInput_Key GetKey(PInputIdentifier type)
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            if (keycodeData[i].IsInput(type))
                return keycodeData[i];
        }
        return null;
    }

    public PInput_ControllerButton GetButton(PInputIdentifier type)
    {
        for (int i = 0; i < buttonData.Count; i++)
        {
            if (buttonData[i].IsInput(type))
                return buttonData[i];
        }
        return null;
    }

    public PInput_ControllerAxis GetAxis(PInputIdentifier type)
    {
        if (axisData == null)
            return null;

        if(axisData.Count > 0)
        {
            for (int i = 0; i < axisData.Count; i++)
            {
                if (axisData[i].IsInput(type))
                    return axisData[i];
            }
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

    private void SetInputs()
    {
        PlayerInputs inputs = GameManager.PlayerInputs;

        ///Setup key list. 
        keycodeData = new List<PInput_Key>()
        {
            new (inputs.key_m_up, PInputIdentifier.M_UP, "MOVE_UP"),
            new (inputs.key_m_down, PInputIdentifier.M_DOWN, "MOVE_DOWN"),
            new (inputs.key_m_left, PInputIdentifier.M_LEFT, "MOVE_LEFT"),
            new (inputs.key_m_right, PInputIdentifier.M_RIGHT, "MOVE_RIGHT"),
            new (inputs.key_a_up, PInputIdentifier.ACTION_UP, "ACTION_UP"),
            new (inputs.key_a_left, PInputIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new (inputs.key_a_down, PInputIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new (inputs.key_a_right, PInputIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        buttonData = new List<PInput_ControllerButton>() {
            new (inputs.ctlr_y, PInputIdentifier.ACTION_UP, "ACTION_UP"),
            new (inputs.ctlr_x, PInputIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new (inputs.ctlr_a, PInputIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new (inputs.ctlr_b, PInputIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        axisData = new List<PInput_ControllerAxis>()
        {
            new (inputs.axis_m_left, "AXIS_LEFT_STICK"),
            new (inputs.axis_a_right, "AXIS_RIGHT_STICK"),
            new (inputs.axis_d_pad, "AXIS_DPAD"),
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
        ClearDelegates();
    }

    private void OnUnloaded(Scene current)
    {
        ClearDelegates();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public static void CreateInstance()
    {
        GameObject obj = new GameObject("-----Menu Player Input System -----");
        obj.AddComponent<PlayerInputSys>();
    }

    public void ClearDelegates()
    {
        foreach (PlayerInput input in inputs)
        {
            input.Clear(); 
        }
    }
}
