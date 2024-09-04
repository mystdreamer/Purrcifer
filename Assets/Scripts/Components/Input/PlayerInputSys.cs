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
    public List<PInput_Key> keycodeData;
    public List<PInput_ControllerButton> buttonData;
    public List<PInput_ControllerAxis> axisData;
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

    private void OnDestroy()
    {
        instance = null;
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

    private void SetInputs()
    {
        ///Setup key list. 
        keycodeData = new List<PInput_Key>()
        {
            new (DefaultInputs.KEY_M_UP, PInputIdentifier.M_UP, "MOVE_UP"),
            new (DefaultInputs.KEY_M_DOWN, PInputIdentifier.M_DOWN, "MOVE_DOWN"),
            new (DefaultInputs.KEY_M_LEFT, PInputIdentifier.M_LEFT, "MOVE_LEFT"),
            new (DefaultInputs.KEY_M_RIGHT, PInputIdentifier.M_RIGHT, "MOVE_RIGHT"),
            new (DefaultInputs.KEY_A_UP, PInputIdentifier.ACTION_UP, "ACTION_UP"),
            new (DefaultInputs.KEY_A_LEFT, PInputIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new (DefaultInputs.KEY_A_DOWN, PInputIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new (DefaultInputs.KEY_A_RIGHT, PInputIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        buttonData = new List<PInput_ControllerButton>() {
            new (DefaultInputs.CTLR_Y, PInputIdentifier.ACTION_UP, "ACTION_UP"),
            new (DefaultInputs.CTLR_X, PInputIdentifier.ACTION_LEFT, "ACTION_LEFT"),
            new (DefaultInputs.CTLR_A, PInputIdentifier.ACTION_DOWN, "ACTION_DOWN"),
            new (DefaultInputs.CTLR_B, PInputIdentifier.ACTION_RIGHT, "ACTION_RIGHT")
        };

        axisData = new List<PInput_ControllerAxis>()
        {
            new (DefaultInputs.AXIS_M_LEFT, "AXIS_LEFT_STICK"),
            new (DefaultInputs.AXIS_A_RIGHT, "AXIS_RIGHT_STICK"),
            new (DefaultInputs.AXIS_DPAD, "AXIS_DPAD"),
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

    public void ClearDelegates()
    {
        foreach (PlayerInput input in inputs)
        {
            input.Clear(); 
        }
    }
}
