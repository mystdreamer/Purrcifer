using UnityEngine;

/// <summary>
/// Serializable data class containing health data. 
/// </summary>
[System.Serializable]
public class PlayerHealthRange
{
    /// <summary>
    /// The minimum range of the pool.
    /// </summary>
    [Header("The minimum health.")]
    [SerializeField] private int min;

    /// <summary>
    /// The maximum range of the pool.
    /// </summary>
    [Header("The maximum health.")]
    [SerializeField] private int max;

    /// <summary>
    /// The maximum range of the pool.
    /// </summary>
    [Header("The current health.")]
    [SerializeField] private int current;

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public int Length => max - min;

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => current > min;

    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public int Health
    {
        get => current; 
        
        set => current = Mathf.Clamp(current + value, min, max);
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public int MaxCap
    {
        get => max;
        set => max = value; 
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public int MinCap
    {
        get => min;
        set => min = value;
    }

    /// <summary>
    /// CTOR. 
    /// </summary>
    /// <param name="min"> The minimum health value of the player. </param>
    /// <param name="max"> The maximum health value of the player. </param>
    /// <param name="current"> The current health of the player. </param>
    public PlayerHealthRange(int min, int max, int current)
    {
        this.min = min;
        this.max = max;
        this.current = current;
    }

    /// <summary>
    /// Returns a default construction of the PlayerHealthRange, used for testing. 
    /// TODO: Remove or wrap in an in editor preprocessor, as should not be relied upon. 
    /// </summary>
    /// <returns> An instance of the PlayerHealthRange. </returns>
    public static PlayerHealthRange GetTestDefault()
    {
        return new PlayerHealthRange(0, 5, 5);
    }
}
