using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enums representing the given actions a player can take. 
/// </summary>
public enum PlayerActionIdentifier
{
    M_LEFT, 
    M_RIGHT, 
    M_UP, 
    M_DOWN,
    A_UP, 
    A_DOWN,
    A_LEFT, 
    A_RIGHT, 
    A_INTERACT
}

/// <summary>
/// Class handling data for player inputs. 
/// </summary>
[System.Serializable]
public class PlayerInput
{
    /// <summary>
    /// The name assigned to the action. 
    /// </summary>
    public string actionName;

    /// <summary>
    /// The key assigned to the action. 
    /// </summary>
    public KeyCode key;

    /// <summary>
    /// The controller button assigned to the action. 
    /// </summary>
    public KeyCode Button;

    /// <summary>
    /// Delegate used by this action. 
    /// </summary>
    /// <param name="result"> Bool representing if the input is active. </param>
    public delegate void MovementAction(bool result);

    /// <summary>
    /// The event to be subscribed to for input notifications. 
    /// </summary>
    public event MovementAction DoAction;

    /// <summary>
    /// The action type assigned. 
    /// </summary>
    public PlayerActionIdentifier type;

    /// <summary>
    /// Function for providing input data to subscribers. 
    /// </summary>
    public void Command_Key()
    {
        if(Input.GetKey(key))
            DoAction?.Invoke(true);
        if(Input.GetKeyUp(key))
            DoAction?.Invoke(false);
    }
}

public class PlayerInputSys : MonoBehaviour
{
    //TODO: Move this later to a better position. 
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
        if(instance == null)
        {
            instance = this; 
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
