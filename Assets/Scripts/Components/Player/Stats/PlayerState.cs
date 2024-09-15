using Purrcifer.PlayerData;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Class responsible for carrying player data statistics. 
/// </summary>
public class PlayerState : MonoBehaviour
{
    private const int IFRAMES = 35;
    private const int MAX_HEALTH_CAP_LIMIT = 12;
    [SerializeField] private PlayerHealthData _healthStats;
    [SerializeField] private PlayerDamageData _damageStats;
    [SerializeField] private PlayerMovementData _movementStats;
    [SerializeField] private PlayerItemData _itemStats;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool deathNotified = false;

    public PlayerDamageData Damage => _damageStats;

    #region Health Properties. 
    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public int Health
    {
        get => _healthStats.current;

        set
        {
            int val = value; 
            if(val > _healthStats.current)
                val = _healthStats.current;
            _healthStats.current = val;
        } 
    }

    public int AddDamage
    {
        set
        {
            if (invincible)
                return;

            int _value = value;
            int sign = MathF.Sign(value);
            if (sign != -1)
                _value *= -1; //Force to be negative. 
            Health += _value;
            invincible = true;
            StartCoroutine(DamageIframes());
        }
    }

    public int AddHealth
    {
        set
        {
            int abs = Mathf.Abs(value);
            Health += abs;
            if (Health > HealthMaxCap)
                Health = HealthMaxCap; 
        }
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public int HealthMaxCap
    {
        get => _healthStats.max;

        set
        {
            _healthStats.max = value;

            //If the value is greater than the allowed max health cap,
            //reset it to the max.
            if (MAX_HEALTH_CAP_LIMIT < _healthStats.max)
                _healthStats.max = MAX_HEALTH_CAP_LIMIT;
        }
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public int HealthMinCap
    {
        get => _healthStats.min;
        set => _healthStats.min = value;
    }

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => (_healthStats.current > _healthStats.min);

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public int Length => HealthMaxCap - HealthMinCap;
    #endregion

    public void SetPlayerData()
    {
        GameManager.Instance.GetPlayerData(ref _healthStats, ref _damageStats);
        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
    }

    private void Update()
    {
        if (_healthStats == null)
            return;
        UIManager.Instance.PlayerHealthBar.UpdateHealthBar(_healthStats.current, _healthStats.max);

        if (!Alive && !deathNotified)
        {
            deathNotified = true;
            Debug.Log("Player Died.");
            GameManager.Instance.PlayerDeath();
        }
    }

    private IEnumerator DamageIframes()
    {
        int iframes = IFRAMES;
        while (0 < iframes)
        {
            //--> Display damage effect. 
            yield return new WaitForEndOfFrame();
            iframes--;
        }

        invincible = false;
    }

    public void ApplyPowerup(Powerup value)
    {
        if(value.WeaponData != null)
        {
            Debug.Log("Implement this. ");
        }

        if (value.UtilityData != null)
        {
            UtilityDataSO so = value.UtilityData;

            _damageStats.BaseDamage += so.damageBase; 
            _damageStats.DamageMultiplier += so.damageMultiplier;
            _damageStats.CriticalHitDamage += so.damageCriticalHit;
            _damageStats.CriticalHitChance += so.damageCriticalChance;
        }

        if (value.ConsumableData != null)
        {
            ConsumableDataSO so = value.ConsumableData;
            Health += so.additiveHealthValue;
        } 
    }
}
