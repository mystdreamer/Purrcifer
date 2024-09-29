using Purrcifer.Data.Defaults;
using UnityEngine;

/// <summary>
/// Container for world state info. 
/// </summary>
[System.Serializable]
public struct WorldStateContainer
{
    /// <summary>
    /// The last WorldState registered.
    /// </summary>
    [SerializeField] private WorldState lastState;
    
    /// <summary>
    /// The current WorldState registered.
    /// </summary>
    [SerializeField] private WorldState currentState;

    /// <summary>
    /// Returns the last WorldState registered.
    /// </summary>
    public WorldState LastState
    {
        get => lastState;
    }

    /// <summary>
    /// The current WorldState registered.
    /// </summary>
    public WorldState CurrentState
    {
        get => currentState;
        set
        {
            lastState = currentState;
            currentState = value;
        }
    }
}
