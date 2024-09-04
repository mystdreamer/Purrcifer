﻿using UnityEngine;

public class BossHealth : MonoBehaviour, IEntityInterface
{
    /// <summary>
    /// The minimum range of the pool.
    /// </summary>
    [Header("The minimum health.")]
    [SerializeField] private float min;

    /// <summary>
    /// The maximum range of the pool.
    /// </summary>
    [Header("The maximum health.")]
    [SerializeField] private float max;

    /// <summary>
    /// The maximum range of the pool.
    /// </summary>
    [Header("The current health.")]
    [SerializeField] private float current;

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public float Length => max - min;

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => current > min;

    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public float Health
    {
        get => current;

        set
        {
            current = value;
            current = Mathf.Clamp(current, min, max);
        }
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public float MaxCap
    {
        get => max;
        set => max = value;
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public float MinCap
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
    public BossHealth(int min, int max, int current)
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
    public static BossHealth GetTestDefault()
    {
        return new BossHealth(0, 5, 5);
    }

    public void ApplyDamage(float value)
    {
        Health -= value;
    }
}