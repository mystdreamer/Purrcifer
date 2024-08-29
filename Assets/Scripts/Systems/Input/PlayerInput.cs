using UnityEngine;
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
