using System;
using UnityEngine;

public class WorldClock : MonoBehaviour
{
    [SerializeField] private float timescaleMinute = WorldTimings.WORLD_TIMESCALE_MINUTE;
    [SerializeField] private float witchingThreshold = WorldTimings.WORLD_WITCHING_HOUR_TIME;
    [SerializeField] private float hellThreshold = WorldTimings.WORLD_HELL_HOUR_TIME;
    [SerializeField] private float time = WorldTimings.WORLD_START_TIME;
    [SerializeField] private float timeReal = WorldTimings.WORLD_START_TIME;
    [SerializeField] private float totalMinutes = 0;
    [SerializeField] private float currentMinutes = 0;
    [SerializeField] private WorldStateEnum currentState = WorldStateEnum.WORLD_START;
    [SerializeField] private WorldStateEnum lastState = WorldStateEnum.WORLD_START;
    [SerializeField] private bool timeReversed = true;

    public WorldStateEnum CurrentState => currentState;
    public WorldStateEnum LastState => lastState;

    public float MinuteLength
    {
        get => timescaleMinute;
        set => timescaleMinute = value;
    }

    public float WitchingThreshold
    {
        get => witchingThreshold;
        set => witchingThreshold = value;
    }

    public float HellThreshold
    {
        get => witchingThreshold;
        set => witchingThreshold = value;
    }

    public bool ReverseTime
    {
        get => timeReversed;
        set => timeReversed = value;
    }

    void Update()
    {
        //Update realtime for tracking. 
        timeReal += Time.deltaTime;
        //Generate the play orientated time. 
        time = (!timeReversed) ? time + Time.deltaTime : time - Time.deltaTime;
        time = Mathf.Clamp(time, 0, 900);

        //If a minute has occurred, the increase minutes and adjust world state. 
        if (time > timescaleMinute)
        {
            currentMinutes++;
            totalMinutes++;
            time -= timescaleMinute;
        }
        UpdateState();
    }

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
}
