using UnityEngine;

[System.Serializable]
public class Range
{
    /// <summary>
    /// The minimum range of the pool.
    /// </summary>
    [Header("The minimum range of the pool")]
    public float min;

    /// <summary>
    /// The maximum range of the pool.
    /// </summary>
    [Header("The maximum range of the pool")]
    public float max;

    /// <summary>
    /// Returns the length between the max and minimum. 
    /// </summary>
    public float Length => max - min;
}
