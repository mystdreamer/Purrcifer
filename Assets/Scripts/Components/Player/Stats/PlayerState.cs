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
    [SerializeField] private PlayerHealthRange _health;
    [SerializeField] private PlayerDamageData _damage;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool deathNotified = false;

    public PlayerDamageData Damage => _damage;

    #region Health Properties. 
    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public int Health
    {
        get => _health.current;

        private set => _health.current = value;
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
        get => _health.max;

        set
        {
            _health.max = value;

            //If the value is greater than the allowed max health cap,
            //reset it to the max.
            if (MAX_HEALTH_CAP_LIMIT < _health.max)
                _health.max = MAX_HEALTH_CAP_LIMIT;
        }
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public int HealthMinCap
    {
        get => _health.min;
        set => _health.min = value;
    }

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => (_health.current > _health.min);

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public int Length => HealthMaxCap - HealthMinCap;
    #endregion

    public void SetPlayerData()
    {
        DataCarrier.Instance.GetPlayerState(ref _health, ref _damage);
        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
    }

    private void Update()
    {
        if (_health == null)
            return;
        UIManager.Instance.PlayerHealthBar.UpdateHealthBar(_health.current, _health.max);

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

    public void ApplyPowerup(PowerupValue value)
    {
        _health.max += value.healthCap; 
        if(value.refillHealth)
            _health.current = _health.max;
        _damage.BaseDamage += value.damageBase; 
        _damage.DamageMultiplier += value.damageMultiplier;
        _damage.CriticalHitChance += value.damageCriticalHit;
        _damage.CriticalHitChance += value.damageCriticalChance; 
    }
}
