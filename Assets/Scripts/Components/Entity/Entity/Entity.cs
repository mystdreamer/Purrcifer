using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityHealth
{
    [SerializeField] private Range healthRange; 
    [SerializeField] private float _invincibilityLength = 0.5F;
    [SerializeField] private bool _invincible;
    [SerializeField] internal bool _bossDamageLock = true;

    public List<HealOverTime> hots = new List<HealOverTime>();
    public List<DamageOverTime> dots = new List<DamageOverTime>();

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public float Length => healthRange.Length;

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => Health > healthRange.min;

    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public float Health
    {
        get => healthRange.current;

        set
        {
            //Check if the applied value is less than the current and return if locked. 
            if (!_bossDamageLock && value < healthRange.current)
                return;

            //Set the value. 
            healthRange.current = value;
            healthRange.Validate();
        }
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public float MaxCap
    {
        get => healthRange.max;
        set => healthRange.max = value;
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public float MinCap
    {
        get => healthRange.min;
        set => healthRange.min = value;
    }

    /// <summary>
    /// Returns true if the entity is invincible. 
    /// </summary>
    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }

    /// <summary>
    /// Returns the remaining invincibility time of the entitity.
    /// </summary>
    public float InvincibilityLength
    {
        get => _invincibilityLength;
        set => _invincibilityLength = value;
    }

    /// <summary>
    /// CTOR. 
    /// </summary>
    /// <param name="min"> The minimum health value of the player. </param>
    /// <param name="max"> The maximum health value of the player. </param>
    /// <param name="current"> The current health of the player. </param>
    public EntityHealth(int min, int max, int current)
    {
        this.healthRange = new Range(current, min, max);
    }
}

public abstract class Entity : WorldObject, IEntityInterface
{
    [SerializeField] private EntityHealth _health;
    public WorldStateContainer container;

    #region Properties. 
    float IEntityInterface.Health
    {
        get => _health.Health;
        set
        {
            if (_health.Invincible || !_health.Alive)
                return;

            float lastValue = _health.Health;
            _health.Health = value;

            HealthChangedEvent(lastValue, _health.Health);

            if (!_health.Alive)
            {
                OnDeathEvent();
                return;
            }

            //Start Iframes.
            if (_health.Health < lastValue && !_health.Invincible)
            {
                InvincibilityActivated();
                StartCoroutine(InvincibilityTimer());
            }
        }
    }

    bool IEntityInterface.IsAlive => _health.Alive;

    public EntityHealth EntityHealthInstance => _health;

    public float CurrentHealth
    {
        get => EntityHealthInstance.Health;
        set => EntityHealthInstance.Health = value;
    }

    public float HealthCap
    {
        get => EntityHealthInstance.MaxCap;
        set => EntityHealthInstance.MaxCap = value;
    }
    #endregion

    public override void WorldUpdateReceiver(WorldState state)
    {
        container.CurrentState = state;
        ApplyWorldState(state);
    }

    private IEnumerator InvincibilityTimer()
    {
        _health.Invincible = true;
        yield return new WaitForSeconds(_health.InvincibilityLength);
        _health.Invincible = false;
    }

    internal void FillHealth()
    {
        CurrentHealth = HealthCap;
    }

    #region H.O.T and D.O.T functions. 

    public void UpdateDotsAndHots()
    {
        bool removeHots = false;
        bool removeDots = false;

        for (int i = 0; i < _health.hots.Count; i++)
        {
            bool ticked = _health.hots[i].Update(Time.deltaTime, ref _health, out bool complete);
            if (complete && !removeHots)
                removeHots = true;
        }

        for (int i = 0; i < _health.dots.Count; i++)
        {
            bool ticked = _health.dots[i].Update(Time.deltaTime, ref _health, out bool complete);
            if (complete && !removeHots)
                removeDots = true;
        }

        if (removeHots)
        {
            List<HealOverTime> currentHots = new List<HealOverTime>();

            for (int i = 0; i < _health.hots.Count; i++)
            {
                if (!_health.hots[i].Completed)
                    currentHots.Add(_health.hots[i]);
            }

            _health.hots = currentHots;
        }

        if (removeDots)
        {
            List<DamageOverTime> currentDots = new List<DamageOverTime>();

            for (int i = 0; i < _health.dots.Count; i++)
            {
                if (!_health.dots[i].Completed)
                    currentDots.Add(_health.dots[i]);
            }

            _health.dots = currentDots;
        }
    }

    public void SetHealOverTime(float time, float healPerTick, float tickEveryX)
    {
        _health.hots.Add(new HealOverTime(time, tickEveryX, healPerTick));
    }

    public void SetDamageOverTime(float time, float damagePerTick, float tickEveryX)
    {
        _health.dots.Add(new DamageOverTime(time, tickEveryX, damagePerTick));
    }

    #endregion

    #region Event Calls.
    internal abstract void ApplyWorldState(WorldState state);

    internal abstract void HealthChangedEvent(float lastValue, float currentValue);

    internal abstract void OnDeathEvent();

    internal abstract void InvincibilityActivated();
    #endregion
}
