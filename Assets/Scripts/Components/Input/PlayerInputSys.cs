using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used for handling player input commands. 
/// </summary>
public class PlayerInputSys : MonoBehaviour
{
    private static PlayerInputSys instance;

    /// <summary>
    /// The current keycode data assigned in the editor. 
    /// </summary>
    public List<PlayerInputKey> keycodeData;
    public List<PlayerInput_ControllerButton> buttonData;
    public List<PlayerInput_ControllerAxis> axisData;
    public PlayerInput[] inputs;

    /// <summary>
    /// Returns the current instance of InputSys. 
    /// </summary>
    public static PlayerInputSys Instance
    {
        get { return instance; }
    }

    void Awake()
    {
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

        Setup();
    }

    public void Setup()
    {
        // Generate Array. 
        List<PlayerInput> list = new List<PlayerInput>();
        list.AddRange(keycodeData);
        list.AddRange(buttonData);
        list.AddRange(axisData);

        inputs = list.ToArray();
    }

    void Update()
    {
        //Update input here. 
        UpdateInput();
    }

    public PlayerInputKey GetKey(PlayerActionIdentifier type) {
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
    public void UpdateInput()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].Command();
        }
    }
}
