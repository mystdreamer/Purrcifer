using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// Class used for containing damage statistics. 
/// </summary>
[System.Serializable]
public struct PlayerDamageData
{
    /// <summary>
    /// The base damage of the player. 
    /// </summary>
    [Header("The base damage.")]
    [Range(0f, 900f)]
    [SerializeField] private float _baseDamage;

    /// <summary>
    /// The damage multiplier applied to the base damage
    /// </summary>
    [Header("The damage multiplier.")]
    [SerializeField] private float _damageMultiplier;

    /// <summary>
    /// Damage applied on critical hits.
    /// </summary>
    [Header("The critical hit damage.")]
    [Range(0f, 900f)]
    [SerializeField] private float _criticalHitDamage;

    /// <summary>
    /// The players critical hit chance.
    /// </summary>
    [Header("The critical hit chance.")]
    [Range(0f, 100f)]
    [SerializeField] private float _criticalHitChance;

    #region Access Properties. 
    /// <summary>
    /// Base damage. 
    /// </summary>
    public float BaseDamage
    {
        get => _baseDamage;
        set => _baseDamage = value;
    }

    /// <summary>
    /// Damage with multipliers applied. 
    /// </summary>
    public float Damage
    {
        get => _baseDamage * _damageMultiplier;
    }

    /// <summary>
    /// The damage multiplier. 
    /// </summary>
    public float DamageMultiplier
    {
        get => _damageMultiplier; 
        set => _damageMultiplier = value;
    }

    /// <summary>
    /// The critical hit damage. 
    /// </summary>
    public float CriticalHitDamage
    {
        get => _criticalHitDamage;
        set => _criticalHitDamage = value;
    }

    /// <summary>
    /// Returns the raw critical hit chance.
    /// </summary>
    public float CriticalHitChance
    {
        get => _criticalHitChance; 
        set => _criticalHitChance = value;
    }

    /// <summary>
    /// Returns true if critical hit should be applied. 
    /// </summary>
    public bool CriticalHitSuccess
    {
        get
        {
            float criticalChanceRemap = math.remap(0, 100, 0, 1, _criticalHitChance);
            float chance = UnityEngine.Random.Range(0, 1);
            return (chance <= criticalChanceRemap);
        }
    }
    #endregion

    public PlayerDamageData(float baseDamage, float damageMultiplier, float criticalHitDamage, float criticalHitChance)
    {
        _baseDamage = baseDamage;
        _damageMultiplier = damageMultiplier;
        _criticalHitDamage = criticalHitDamage;
        _criticalHitChance = criticalHitChance;
    }
}
