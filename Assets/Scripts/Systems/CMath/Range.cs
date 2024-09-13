using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Range
{
    /// <summary>
    /// The minimum range.
    /// </summary>
    [Header("The minimum range.")]
    public float min;

    /// <summary>
    /// The maximum range.
    /// </summary>
    [Header("The maximum range.")]
    public float max;

    /// <summary>
    /// The maximum range.
    /// </summary>
    [Header("The current value.")]
    public float current;

    /// <summary>
    /// Returns the length between the max and minimum. 
    /// </summary>
    public float Length => max - min;

    public Range(float current, float min, float max)
    {
        this.current = current;
        this.min = min;
        this.max = max;
    }

    public void Validate()
    {
        if (current < min)
            current = min;
        if (current > max)
            current = max;
    }
}
