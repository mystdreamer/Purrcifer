using UnityEngine;

/// <summary>
/// Class containing timings used to control the time of the world. 
/// </summary>
public class WorldTimings
{
    public const float WORLD_TIMESCALE_MINUTE = 5;
    public const float WORLD_START_TIME = 0.0f;
    public const float WORLD_WITCHING_HOUR_TIME = 5.0F;
    public const float WORLD_HELL_HOUR_TIME = 10.0F; 
}

/// <summary>
/// Enum states for the world. 
/// </summary>
[System.Serializable]
public enum WorldStateEnum : int
{
    WORLD_START = 0, 
    WORLD_WITCHING = 1, 
    WORLD_HELL = 2
}
