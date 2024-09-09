using Purrcifer.Data.Defaults;
using UnityEditor.Compilation;
using UnityEngine;

[System.Serializable]
public class EHealth
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
            if (current < min)
                current = min;
            if (current > max)
                current = max;
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
    public EHealth(int min, int max, int current)
    {
        this.min = min;
        this.max = max;
        this.current = current;
    }
}

[System.Serializable]
public class EDamage
{
    public int normalDamage;
    public int witchingDamage;
    public int hellDamage;
}

public abstract class Entity : MonoBehaviour, IEntityInterface
{
    [SerializeField] private EHealth health;
    [SerializeField] private EDamage damage;
    
    float IEntityInterface.Health {
        get => health.Health;
        set => health.Health = value;
    }

    bool IEntityInterface.IsAlive => health.Alive;

    void IEntityInterface.ApplyWorldState(WorldStateEnum state) => ApplyWorldState(state);

    internal abstract void ApplyWorldState(WorldStateEnum state);
}
