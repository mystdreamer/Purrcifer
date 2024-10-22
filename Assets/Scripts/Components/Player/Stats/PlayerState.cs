using Purrcifer.Data.Player;
using Purrcifer.PlayerData;
using System;
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
    /// Flag denoting player invinability. 
    /// </summary>
    public bool Invincible
    {
        get => invincible;
        set => invincible = value;
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

    #region Damage Properties

    /// <summary>
    /// Base damage. 
    /// </summary>
    public float BaseDamage
    {
        get => _damageStats.BaseDamage;
        set => _damageStats.BaseDamage = value;
    }

    /// <summary>
    /// The damage multiplier. 
    /// </summary>
    public float DamageMultiplier
    {
        get => _damageStats.DamageMultiplier;
        set => _damageStats.DamageMultiplier = value;
    }

    /// <summary>
    /// The critical hit damage. 
    /// </summary>
    public float CriticalHitDamage
    {
        get => _damageStats.CriticalHitDamage;
        set => _damageStats.CriticalHitDamage = value;
    }

    /// <summary>
    /// Returns the raw critical hit chance.
    /// </summary>
    public float CriticalHitChance
    {
        get => _damageStats.CriticalHitChance;
        set => _damageStats.CriticalHitChance = value;
    }

    /// <summary>
    /// Returns true if critical hit should be applied. 
    /// </summary>
    public bool CriticalHitSuccess => _damageStats.CriticalHitSuccess;

    /// <summary>
    /// Returns the precalculated damage from the player.
    /// </summary>
    public float Damage
    {
        get => _damageStats.Damage + ((_damageStats.CriticalHitSuccess) ? _damageStats.CriticalHitDamage : 0);
    }

    public float AttackRate { 
        get => _damageStats.AttackRate; 
    }
    #endregion

    #region Powerup/Consumable Accessors. 
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

    public Powerup SetPowerup
    {
        set => ApplyPowerup(value);
    }

    public StatUpgradeDataSO ApplyStatUpgrade
    {
        set => ApplyPowerup(value);
    }

    public UtilityDataSO ApplyUtilityUpgrade
    {
        set => ApplyPowerup(value);
    }

    public WeaponDataSO ApplyWeaponUpgrade
    {
        set => ApplyPowerup(value);
    }

    public ConsumableDataSO ApplyConsumable
    {
        set => ApplyPowerup(value);
    }
    #endregion

    public void SetPlayerData(GameSaveFileRuntime runtime)
    {
        _healthStats = new PlayerHealthData() {
            min = runtime.minHealth, 
            max = runtime.maxHealth, 
            current = runtime.currentHealth
        };

        _itemStats = new PlayerItemData()
        {
            talismanCharges = runtime.talismanCount, 
            utilityCharges = runtime.utilityCharges,
        };

        _damageStats = new PlayerDamageData()
        {
            BaseDamage = runtime.baseDamage, 
            DamageMultiplier = runtime.damageMultiplier, 
            CriticalHitDamage = runtime.criticalHitDamage,
            CriticalHitChance = runtime.criticalHitChance,
            AttackRate = runtime.attackRate,
        };

        _movementStats = new PlayerMovementData()
        {
            moveSpeed = runtime.movementSpeed,
        };

        UIManager.Instance.PlayerHealthBar.HealthBarEnabled = true;
        UIManager.Instance.PlayerTalismans.EnableDisplay = true;
    }

    public PlayerDamageData GetDamageData() => _damageStats;

    public void GetPlayerData(GameSaveFileRuntime runtime)
    {
        runtime.minHealth = HealthMinCap;
        runtime.maxHealth = HealthMaxCap;
        runtime.attackRate = AttackRate;
        runtime.currentHealth = Health;
        runtime.baseDamage = BaseDamage;
        runtime.damageMultiplier = DamageMultiplier;
        runtime.criticalHitDamage = CriticalHitDamage;
        runtime.criticalHitChance = CriticalHitChance;
        runtime.talismanCount = _itemStats.talismanCharges; 
        runtime.utilityCharges = _itemStats.utilityCharges;
        runtime.movementSpeed = _movementStats.moveSpeed;
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

    #region Upgrade Functions.
    private void ApplyPowerup(Powerup value)
    {
        if (value.WeaponData != null)
            Debug.Log("Powerup: Weapon Data: Implement this. ");

        if (value.UtilityData != null)
            ApplyPowerup(value.UtilityData);

        if (value.StatUpgradeItemData != null)
            ApplyPowerup(value.StatUpgradeItemData);

        if (value.ConsumableData != null)
            ApplyPowerup(value.ConsumableData);
    }

    private void ApplyPowerup(UtilityDataSO data)
    {
        switch (data.type)
        {
            case UtilityDataSO.UtilityType.STOPWATCH:
                Stopwatch stpWatch = gameObject.AddComponent<Stopwatch>();
                stpWatch.charge = new Range(_itemStats.utilityCharges, 0, _itemStats.utilityCharges);
                break;
        }

        ApplyUpgradeValueTypes(data.statChangeInts);
        ApplyUpgradeValueTypes(data.statChangeFloats);
    }

    private void ApplyPowerup(StatUpgradeDataSO data)
    {
        ApplyUpgradeValueTypes(data.statChangeInts);
        ApplyUpgradeValueTypes(data.statChangeFloats);
        ApplyUpgradeValueTypes(data.statChangeEffects);
    }

    private void ApplyPowerup(ConsumableDataSO data)
    {
        ApplyConsumableValueTypes(data.intEffects);
        ApplyConsumableValueTypes(data.boolEffects);
    }

    private void ApplyPowerup(WeaponDataSO data)
    {
        //Select weapon type: 
        switch (data.type)
        {
            case WeaponDataSO.WeaponType.SWORD:
                SwordAttack attack = gameObject.AddComponent<SwordAttack>();
                attack.prefabs = new Weapon_4DirectionalPrefabs();
                attack.prefabs.up = data.weaponPrefabUp;
                attack.prefabs.down = data.weaponPrefabDown;
                attack.prefabs.left = data.weaponPrefabLeft;
                attack.prefabs.right = data.weaponPrefabRight;
                attack.prefabs.destructionTime = data.destructionTime;
                attack.prefabs.cooldownTime = data.cooldownTime;
                break;
        }

        ApplyUpgradeValueTypes(data.statChangeInts);
        ApplyUpgradeValueTypes(data.statChangeFloats);
        ApplyUpgradeValueTypes(data.statChangeEffects);
    }

    private void ApplyUpgradeValueTypes(ItemDataSO.StatChangeInt[] type)
    {
        for (int i = 0; i < type.Length; i++)
        {
            switch (type[i].type)
            {
                case ItemDataSO.IntItemValueType.HEALTH_CAP:
                    _healthStats.max += type[i].value;
                    break;
                case ItemDataSO.IntItemValueType.PLAYER_UTILITY_CHARGE:
                    _itemStats.utilityCharges += type[i].value;
                    break;
            }
        }
    }

    private void ApplyUpgradeValueTypes(ItemDataSO.StatChangeFloat[] type)
    {

        for (int i = 0; i < type.Length; i++)
        {
            switch (type[i].type)
            {
                case ItemDataSO.FloatItemValueType.MOVEMENT_SPEED:
                    _movementStats.moveSpeed += type[i].value;
                    break;
                case ItemDataSO.FloatItemValueType.DAMAGE_BASE:
                    _damageStats.BaseDamage += type[i].value;
                    break;
                case ItemDataSO.FloatItemValueType.DAMAGE_MULTIPLIER:
                    _damageStats.DamageMultiplier += type[i].value;
                    break;
                case ItemDataSO.FloatItemValueType.DAMAGE_CRITICAL_HIT:
                    _damageStats.CriticalHitDamage += type[i].value;
                    break;
                case ItemDataSO.FloatItemValueType.DAMAGE_CRITICAL_CHANCE:
                    _damageStats.CriticalHitChance += type[i].value;
                    break;
                case ItemDataSO.FloatItemValueType.ATTACK_RATE:
                    _damageStats.AttackRate += type[i].value;
                    break;
            }
        }
    }

    private void ApplyUpgradeValueTypes(ItemDataSO.StatChangeEffect[] type)
    {
        for (int i = 0; i < type.Length; i++)
        {
            switch (type[i].type)
            {
                case ItemDataSO.BoolItemValueType.FILL_HEALTH:
                    _healthStats.current = HealthMaxCap;
                    break;
                case ItemDataSO.BoolItemValueType.REDUCE_HEALTH_TO_ONE:
                    _healthStats.current = 1; 
                    break;
                default:
                    break;
            }
        }
    }

    private void ApplyConsumableValueTypes(ConsumableDataSO.BoolConsumableEffect[] effects)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            switch (effects[i].type)
            {
                case ConsumableDataSO.ConsumableEffectBool.FILL_HEALTH:
                    _healthStats.current = _healthStats.max;
                    break;
                default:
                    break;
            }
        }
    }

    private void ApplyConsumableValueTypes(ConsumableDataSO.IntConsumableEffect[] effects)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            switch (effects[i].effect)
            {
                case ConsumableDataSO.ConsumableEffectInt.ADD_HEALTH:
                    _healthStats.current += effects[i].value;
                    break;
                case ConsumableDataSO.ConsumableEffectInt.REMOVE_HEALTH:
                    _healthStats.current -= effects[i].value;
                    break;
                case ConsumableDataSO.ConsumableEffectInt.ADD_TALISMAN:
                    _itemStats.talismanCharges += effects[i].value;
                    break;
                case ConsumableDataSO.ConsumableEffectInt.ADD_CHARGE:
                    _itemStats.utilityCharges += effects[i].value;
                    break;
            }
        }
    }
    #endregion
}
