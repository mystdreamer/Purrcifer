﻿using UnityEngine;

[System.Serializable]
public abstract class PlayerInput
{
    /// <summary>
    /// The name assigned to the action. 
    /// </summary>
    public string actionName;

    /// <summary>
    /// The action type assigned. 
    /// </summary>
    public PlayerActionIdentifier type;

    public bool IsInput(PlayerActionIdentifier actionIdentifier)
    {
        return (int)type == (int)actionIdentifier;
    }

    /// <summary>
    /// Function for providing input data to subscribers. 
    /// </summary>
    public abstract void Command();
}

/// <summary>
/// Class handling data for player inputs. 
/// </summary>
[System.Serializable]
public class PlayerInputKey : PlayerInput
{
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
    /// The key assigned to the action. 
    /// </summary>
    public KeyCode key;

    public override void Command()
    {
        if (Input.GetKey(key))
            DoAction?.Invoke(true);
        if (Input.GetKeyUp(key))
            DoAction?.Invoke(false);
    }
}

[System.Serializable]
public class PlayerInput_ControllerButton : PlayerInput
{
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
    /// The controller button assigned to the action. 
    /// </summary>
    public KeyCode Button;

    public override void Command()
    {
        if (Input.GetKey(Button))
            DoAction?.Invoke(true);
        if (Input.GetKeyUp(Button))
            DoAction?.Invoke(false);
    }
}

[System.Serializable]
public class PlayerInput_ControllerAxis : PlayerInput
{
    /// <summary>
    /// Delegate used by this action. 
    /// </summary>
    /// <param name="result"> Bool representing if the input is active. </param>
    public delegate void MovementAction(Vector3 result);

    /// <summary>
    /// The event to be subscribed to for input notifications. 
    /// </summary>
    public event MovementAction DoAction;

    public override void Command()
    {
        float axisValueX = Input.GetAxis("Horizontal");
        float axisValueZ = Input.GetAxis("Vertical");
        float deadZone = 0.15f;
        Vector3 direction = new Vector3(axisValueX, 0, axisValueZ);
        Vector3 returnVal = Vector3.zero; 

        if (axisValueX > deadZone || axisValueX < -deadZone)
        {
            returnVal.x = direction.x;
        }

        if (axisValueZ > deadZone || axisValueZ < -deadZone)
        {
            returnVal.z = direction.z;
        }

        DoAction?.Invoke(returnVal.normalized);
    }
}