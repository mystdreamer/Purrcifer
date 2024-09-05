using UnityEngine;

public class EntityHealth : MonoBehaviour, IEntityInterface
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
            if(current < min) 
                current = min;
            if(current > max)
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
    public EntityHealth(int min, int max, int current)
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
    public static EntityHealth GetTestDefault()
    {
        return new EntityHealth(0, 5, 5);
    }

    public void ApplyDamage(float value)
    {
        Debug.Log("Applying damage.");
;        Health -= value; 
    }
}
