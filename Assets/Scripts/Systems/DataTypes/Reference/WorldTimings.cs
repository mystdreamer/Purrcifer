using UnityEngine;

/// <summary>
/// Class containing timings used to control the time of the world. 
/// </summary>
public class WorldTimings
{
    /// <summary>
    /// The scale of time per minute. 
    /// </summary>
    public const float WORLD_TIMESCALE_MINUTE = 60;

    /// <summary>
    /// When world time actually starts. 
    /// </summary>
    public const float WORLD_START_TIME = 0.0f;

    /// <summary>
    /// The threshold for ticking over into witching hour. 
    /// </summary>
    public const float WORLD_WITCHING_HOUR_TIME = 5.0F;

    /// <summary>
    /// The threshold for ticking over into hell hour. 
    /// </summary>
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
