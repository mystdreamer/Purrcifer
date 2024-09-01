using System;
using UnityEngine;

public class WorldClock : MonoBehaviour
{
    #region Fields. 
    /// <summary>
    /// The scale of time per minute. 
    /// </summary>
    [SerializeField] private float timescaleMinute = WorldTimings.WORLD_TIMESCALE_MINUTE;

    /// <summary>
    /// The threshold for ticking over into witching hour. 
    /// </summary>
    [SerializeField] private float witchingThreshold = WorldTimings.WORLD_WITCHING_HOUR_TIME;

    /// <summary>
    /// The threshold for ticking over into hell hour. 
    /// </summary>
    [SerializeField] private float hellThreshold = WorldTimings.WORLD_HELL_HOUR_TIME;

    /// <summary>
    /// The timing interval used for playing. 
    /// </summary>
    [SerializeField] private float timePlay = WorldTimings.WORLD_START_TIME;

    /// <summary>
    /// The timing interval used for real time calculations. 
    /// </summary>
    [SerializeField] private float timeReal = WorldTimings.WORLD_START_TIME;

    /// <summary>
    /// The total number of minutes passed. 
    /// </summary>
    [SerializeField] private float totalMinutes = 0;
    
    /// <summary>
    /// The total number of minutes passed in the modifiable game time. 
    /// </summary>
    [SerializeField] private float currentMinutes = 0;
    
    /// <summary>
    /// The current state of the world. 
    /// </summary>
    [SerializeField] private WorldStateEnum currentState = WorldStateEnum.WORLD_START;
    
    /// <summary>
    /// The last state of the world. 
    /// </summary>
    [SerializeField] private WorldStateEnum lastState = WorldStateEnum.WORLD_START;
    
    /// <summary>
    /// Whether play time is reversed. 
    /// </summary>
    [SerializeField] private bool timeReversed = false;
    #endregion

    #region Properties. 
    /// <summary>
    /// Returns the current state of the world.
    /// </summary>
    public WorldStateEnum CurrentState => currentState;

    /// <summary>
    /// Returns the last state of the world. 
    /// </summary>
    public WorldStateEnum LastState => lastState;

    /// <summary>
    /// The current value used as the base for a minute. 
    /// </summary>
    public float MinuteLength
    {
        get => timescaleMinute;
        set => timescaleMinute = value;
    }

    /// <summary>
    /// The current value set as the witching time threshold. 
    /// </summary>
    public float WitchingThreshold
    {
        get => witchingThreshold;
        set => witchingThreshold = value;
    }

    /// <summary>
    /// The current value set as the hell time threshold. 
    /// </summary>
    public float HellThreshold
    {
        get => witchingThreshold;
        set => witchingThreshold = value;
    }

    /// <summary>
    /// Reverses the flow of the game time. 
    /// </summary>
    public bool ReversePlayTime
    {
        get => timeReversed;
        set => timeReversed = value;
    }

    /// <summary>
    /// The current play time (time modified by play). 
    /// </summary>
    public float PlayTime
    {
        get => timePlay; 
        
        set
        {
            timePlay = value;
            RecalculateTime();
        }
    }

    /// <summary>
    /// The current real time the game has been running for. 
    /// </summary>
    public float RealTime
    {
        get => timeReal;
    }

    /// <summary>
    /// Set whether the game timer is active or not. 
    /// </summary>
    public bool TimerActive
    {
        get;
        set;
    } = false;

    #endregion

    void Update()
    {
        if (!TimerActive)
            return; 

        //Update real time for tracking. 
        timeReal += Time.deltaTime;
        //Generate the play orientated time. 
        timePlay = (!timeReversed) ? timePlay + Time.deltaTime : timePlay - Time.deltaTime;
        timePlay = Mathf.Clamp(timePlay, 0, 900);
        RecalculateTime();
    }

    /// <summary>
    /// Used to recalculate time post a tick event. 
    /// </summary>
    private void RecalculateTime()
    {
        //If a minute has occurred, the increase minutes and adjust world state. 
        if (timePlay > timescaleMinute)
        {
            totalMinutes++;
            timePlay -= timescaleMinute;
            UpdateState();
        }

        if (timeReal > timescaleMinute)
        {
            totalMinutes++;
            timeReal -= timescaleMinute;
        }

    }

    /// <summary>
    /// Updates the state of the world based on the current play time. 
    /// </summary>
    private void UpdateState()
    {
        lastState = currentState;

        if (currentMinutes < witchingThreshold)
            currentState = WorldStateEnum.WORLD_START;

        if (currentMinutes >= witchingThreshold)
            currentState = WorldStateEnum.WORLD_WITCHING;

        if (currentMinutes >= hellThreshold)
            currentState = WorldStateEnum.WORLD_HELL;
    }

    /// <summary>
    /// Resets the current game time. 
    /// </summary>
    public void ResetPlayTime()
    {
        timePlay = 0;
        lastState = currentState = WorldStateEnum.WORLD_START;
    }
}
