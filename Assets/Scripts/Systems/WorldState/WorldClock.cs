using JetBrains.Annotations;
using Purrcifer.Data.Defaults;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Data struct for handling game time. 
/// </summary>
[System.Serializable]
public class TimeData
{
    /// <summary>
    /// The scale of time per minute. 
    /// </summary>
    private float timescaleMinute;

    /// <summary>
    /// The scale of time per hour. 
    /// </summary>
    private float timeScaleHour;

    /// <summary>
    /// The current number of seconds. 
    /// </summary>
    public float seconds;

    /// <summary>
    /// The current number of minutes. 
    /// </summary>
    public int minutes;

    /// <summary>
    /// The current number of hours. 
    /// </summary>
    public int hours;

    /// <summary>
    /// Get/Set the current time of data class. 
    /// </summary>
    public float Time
    {
        get => seconds;
        set
        {
            seconds = value;
            RecalculateTime();
        }
    }

    public TimeData(float time, int seconds, int minutes, int hours)
    {
        timescaleMinute = WorldTimings.WORLD_TIMESCALE_MINUTE;
        timeScaleHour = WorldTimings.WORLD_WITCHING_HOUR_TIME;
        this.seconds = time;
        this.minutes = minutes;
        this.seconds = seconds;
        this.hours = hours;

    }

    public virtual void UpdateTime(float dt)
    {
        seconds += dt;
        RecalculateTime();
    }

    public void RecalculateTime()
    {
        //If a minute has occurred, the increase minutes and adjust world state. 
        if (seconds > timescaleMinute)
        {
            minutes++;
            seconds -= timescaleMinute;

            if (minutes > timeScaleHour)
                hours++;
        }
    }
}

public class PlayTimeData : TimeData
{
    /// <summary>
    /// Denotes that the flow of play time is reversed. 
    /// </summary>
    public bool timeReversed;

    /// <summary>
    /// Denotes that the flow of time is currently fast forwarded. 
    /// </summary>
    public bool timeFastForward;

    public PlayTimeData(float time, int seconds, int minutes, int hours) : base(time, seconds, minutes, hours)
    {
        timeReversed = false;
        timeFastForward = false;
    }

    public override void UpdateTime(float dt)
    {
        if (timeReversed)
            seconds -= dt;
        else if (timeFastForward)
            seconds += (dt * 2); //Probably a better, more stable way to handle. 
        else
            seconds += dt;

        RecalculateTime();
    }
}

public class WorldClock : MonoBehaviour
{
    #region Fields. 

    public TimeData realTime = new TimeData(0, 0, 0, 0);
    public PlayTimeData playTime = new PlayTimeData(0, 0, 0, 0);
    
    /// <summary>
    /// The threshold for ticking over into witching hour. 
    /// </summary>
    [SerializeField] private float _witchingThreshold = WorldTimings.WORLD_WITCHING_HOUR_TIME;

    /// <summary>
    /// The threshold for ticking over into hell hour. 
    /// </summary>
    [SerializeField] private float _hellThreshold = WorldTimings.WORLD_HELL_HOUR_TIME;

    /// <summary>
    /// The current state of the world. 
    /// </summary>
    [SerializeField] private WorldState _currentState = WorldState.WORLD_START;

    /// <summary>
    /// The last state of the world. 
    /// </summary>
    [SerializeField] private WorldState _lastState = WorldState.WORLD_START;
    
    float reverseSection = 0;
    bool removingTime = false;
    bool removalValueChanged = false;
    float additionSection = 0;
    bool addingTime = false;
    bool additionValueChanged = false;
    #endregion

    #region Properties. 
    /// <summary>
    /// Returns the current state of the world.
    /// </summary>
    public WorldState CurrentState => _currentState;

    /// <summary>
    /// Returns the last state of the world. 
    /// </summary>
    public WorldState LastState => _lastState;

    /// <summary>
    /// The current value set as the witching time threshold. 
    /// </summary>
    public float WitchingThreshold
    {
        get => _witchingThreshold;
        set => _witchingThreshold = value;
    }

    /// <summary>
    /// The current value set as the hell time threshold. 
    /// </summary>
    public float HellThreshold
    {
        get => _witchingThreshold;
        set => _witchingThreshold = value;
    }

    /// <summary>
    /// Reverses the flow of the game time. 
    /// </summary>
    public bool ReversePlayTime
    {
        get => playTime.timeReversed;
        set => playTime.timeReversed = value;
    }

    public bool FFWDPlayTime
    {
        get => playTime.timeFastForward;
        set => playTime.timeFastForward = value;
    }

    /// <summary>
    /// The current play time (time modified by play). 
    /// </summary>
    public float PlayTime
    {
        get => playTime.seconds;

        set
        {
            playTime.seconds = value;
            UpdateWorldState();
        }
    }

    /// <summary>
    /// The current real time the game has been running for. 
    /// </summary>
    public float RealTime
    {
        get => realTime.seconds;
    }

    /// <summary>
    /// Set whether the game timer is active or not. 
    /// </summary>
    public bool TimerActive
    {
        get;
        set;
    } = false;

    public bool RemoveOpActive => removingTime;
    public bool AdditionOpActive => addingTime;

    #endregion

    private void Start()
    {
        _witchingThreshold = WorldTimings.WORLD_WITCHING_HOUR_TIME;
        _hellThreshold = WorldTimings.WORLD_HELL_HOUR_TIME;
        _currentState = WorldState.WORLD_START;
        _lastState = WorldState.WORLD_START;
    }

    void Update()
    {
        if (!TimerActive)
            return;

        //Update real time. 
        realTime.UpdateTime(Time.deltaTime);
        //Update play time. 
        playTime.UpdateTime(Time.deltaTime);

        //Update the current world state. 
        UpdateWorldState();
    }

    /// <summary>
    /// Update the current world state. 
    /// </summary>
    private void UpdateWorldState()
    {
        _lastState = _currentState;

        if (playTime.minutes < _witchingThreshold)
            _currentState = WorldState.WORLD_START;

        if (playTime.minutes >= _witchingThreshold)
            _currentState = WorldState.WORLD_WITCHING;

        if (playTime.minutes >= _hellThreshold)
            _currentState = WorldState.WORLD_HELL;

        if (_currentState != _lastState)
        {
            GameManager.Instance.WorldStateChange?.Invoke(_currentState);
            _lastState = _currentState;
        }
    }

    /// <summary>
    /// Resets the current game time. 
    /// </summary>
    public void ResetPlayTime()
    {
        playTime.Time = 0;
        _lastState = _currentState = WorldState.WORLD_START;
    }

    #region Play time modifiers. 
    public void RemoveValue(float value)
    {
        float valueAbs = MathF.Abs(value);

        if (!removingTime)
        {
            removingTime = true;
            reverseSection = valueAbs;
            StartCoroutine(ReduceTime());
        }
        else
        {
            reverseSection += valueAbs;
            removalValueChanged = true;
        }
    }

    private IEnumerator ReduceTime()
    {
        float initialValue = PlayTime;
        float endValue = initialValue - reverseSection;

        while (!(PlayTime <= endValue))
        {
            //Need to recalculate. 
            if (removalValueChanged)
            {
                //Calculate the amount changed prior.
                endValue = initialValue - reverseSection;
            }
            //Reduce the current playtime and wait.
            PlayTime -= 0.1F;
            UpdateWorldState();
            yield return new WaitForSeconds(0.002F);
        }

        if (PlayTime <= 0)
            PlayTime = 0;
        removingTime = false;
    }

    public void AddValue(float value)
    {
        float valueAbs = MathF.Abs(value);

        if (!addingTime)
        {
            addingTime = true;
            additionSection = valueAbs;
            StartCoroutine(AddTime());
        }
        else
        {
            reverseSection += valueAbs;
            additionValueChanged = true;
        }
    }

    private IEnumerator AddTime()
    {
        float initialValue = PlayTime;
        float endValue = initialValue + additionSection;

        while (PlayTime > endValue)
        {
            //Need to recalculate. 
            if (additionValueChanged)
            {
                //Calculate the amount changed prior.
                endValue = initialValue + additionSection;
            }
            //Reduce the current playtime and wait.
            PlayTime += 0.1F;
            UpdateWorldState();
            yield return new WaitForEndOfFrame();
        }

        UpdateWorldState();
        addingTime = false;
    }

    #endregion
}