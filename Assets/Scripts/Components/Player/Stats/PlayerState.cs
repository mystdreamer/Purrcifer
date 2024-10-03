using Purrcifer.PlayerData;
using System.Collections;
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
            if (val > _healthStats.current)
                val = _healthStats.current;
            else if (val < _healthStats.current)
            {
                if (invincible)
                    return;

                _healthStats.current = val;
                invincible = true;
                StartCoroutine(DamageIframes());
            }
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

    public int Talismans
    {
        get => _itemStats.talismanCharges;

        set
        {
            _itemStats.talismanCharges = value;
            if (_itemStats.talismanCharges < 0)
                _itemStats.talismanCharges = 0;
        }
    }

    public void SetPlayerData()
    {
        _healthStats = GameManager.Instance.GetPlayerHealthData;
        _damageStats = GameManager.Instance.GetPlayerDamageData;
        _itemStats = GameManager.Instance.GetPlayerItemData;
        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
        UIManager.Instance.PlayerTalismans.Enable();
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
        if (value.WeaponData != null)
            Debug.Log("Powerup: Weapon Data: Implement this. ");

        if (value.UtilityData != null)
            ApplyPowerup(value.UtilityData);

        if (value.ConsumableData != null)
            ApplyConsumable(value.ConsumableData);
    }

    public void ApplyPowerup(UtilityDataSO data)
    {
        Debug.Log("Powerup application called");
        _healthStats.max += data.healthCap;
        //_movementStats.moveSpeed += data.playerSpeed;
        //_itemStats.utilityCharges += data.playerCharge;
        _damageStats.BaseDamage += data.damageBase;
        _damageStats.DamageMultiplier += data.damageMultiplier;
        _damageStats.CriticalHitDamage += data.damageCriticalHit;
        _damageStats.CriticalHitChance += data.damageCriticalChance;

        if (data.refillHealth)
            Health = HealthMaxCap;
    }

    public void ApplyConsumable(ConsumableDataSO data)
    {
        Health += data.additiveHealthValue;
        _itemStats.talismanCharges += data.additiveTalismanValue;
    }
}
