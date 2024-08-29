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
    public List<PlayerInput> keycodeData;

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
        if(instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    void Update()
    {
        //Update input here. 
        UpdateInput();
    }

    /// <summary>
    /// Gets an action from the manager. 
    /// </summary>
    /// <param name="type"> The action type to return. </param>
    /// <returns></returns>
    public PlayerInput GetAction(PlayerActionIdentifier type)
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            if (keycodeData[i].type == type)
                return keycodeData[i];
        }

        return null;
    }

    /// <summary>
    /// Update all inputs in the system. 
    /// </summary>
    public void UpdateInput()
    {
        for (int i = 0; i < keycodeData.Count; i++)
        {
            keycodeData[i].Command_Key();
        }
    }
}
