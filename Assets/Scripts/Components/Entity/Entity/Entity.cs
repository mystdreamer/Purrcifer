using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityHealth
{
    [SerializeField] private Range _healthRange; 
    [SerializeField] private List<HealOverTime> _hots = new List<HealOverTime>();
    [SerializeField] private List<DamageOverTime> _dots = new List<DamageOverTime>();
    [SerializeField] private bool _invincible; 
    [SerializeField] private float _invincibilityLength = 0.5F;

    /// <summary>
    /// Returns the total value of the players health. 
    /// </summary>
    public float Length => _healthRange.Length;

    /// <summary>
    /// Returns true if the player is alive. 
    /// </summary>
    public bool Alive => Health > _healthRange.min;

    /// <summary>
    /// Returns the players current health. 
    /// </summary>
    public float Health
    {
        get => _healthRange.current;

        set
        {
            //Set the value. 
            _healthRange.current = value;
            _healthRange.Validate();
        }
    }

    /// <summary>
    /// Returns the maximum cap for the players health. 
    /// </summary>
    public float MaxCap
    {
        get => _healthRange.max;
        set => _healthRange.max = value;
    }

    /// <summary>
    /// Returns the minimum cap for the players health. 
    /// </summary>
    public float MinCap
    {
        get => _healthRange.min;
        set => _healthRange.min = value;
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

    public List<HealOverTime> Hots => _hots;

    public List<DamageOverTime> Dots => _dots;

    public void SetHealOverTime(HealOverTime hot) => _hots.Add(hot);

    public void SetDamageOverTime(DamageOverTime dot) => _dots.Add(dot);

    public void SetDamageOverTime(float time, float damagePerTick, float tickEveryX)
    {
        _dots.Add(new DamageOverTime(time, tickEveryX, damagePerTick));
    }

    public void SetHealOverTime(float time, float healPerTick, float tickEveryX)
    {
        _hots.Add(new HealOverTime(time, tickEveryX, healPerTick));
    }

    public void SetDamageOverTime(float time, float damagePerTick, float tickEveryX)
    {
        _dots.Add(new DamageOverTime(time, tickEveryX, damagePerTick));
    }

    public static void ApplyBuffs(ref EntityHealth health)
    {
        bool cleanHots = false;
        bool cleanDots = false;

        for (int i = 0; i < health._hots.Count; i++)
        {
            bool ticked = health._hots[i].Update(Time.deltaTime, ref health, out bool complete);
            if (complete && !cleanHots)
                cleanHots = true;
        }

        for (int i = 0; i < health._dots.Count; i++)
        {
            bool ticked = health._dots[i].Update(Time.deltaTime, ref health, out bool complete);
            if (complete && !cleanHots)
                cleanDots = true;
        }

        if(cleanHots) health.CleanHots();
        if(cleanDots) health.CleanDots();
    }

    private void CleanHots()
    {
        List<HealOverTime> currentHots = new List<HealOverTime>();

        for (int i = 0; i < _hots.Count; i++)
            if (!_hots[i].Completed) currentHots.Add(_hots[i]);

        _hots = currentHots;
    }

    private void CleanDots()
    {
        List<DamageOverTime> currentDots = new List<DamageOverTime>();

        for (int i = 0; i < _dots.Count; i++)
            if (!_dots[i].Completed) currentDots.Add(_dots[i]);

        _dots = currentDots;
    }

    /// <summary>
    /// CTOR. 
    /// </summary>
    /// <param name="min"> The minimum health value of the player. </param>
    /// <param name="max"> The maximum health value of the player. </param>
    /// <param name="current"> The current health of the player. </param>
    public EntityHealth(int min, int max, int current)
    {
        this._healthRange = new Range(current, min, max);
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

    public HealOverTime SetHOT { 
        set => _health.SetHealOverTime(value); 
    }

    public DamageOverTime SetDOT
    {
        set => _health.SetDamageOverTime(value);
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

    internal void UpdateDots() => EntityHealth.ApplyBuffs(ref _health);

    #region Event Calls.
    internal abstract void ApplyWorldState(WorldState state);

    internal abstract void HealthChangedEvent(float lastValue, float currentValue);

    internal abstract void OnDeathEvent();

    internal abstract void InvincibilityActivated();
    #endregion
}
