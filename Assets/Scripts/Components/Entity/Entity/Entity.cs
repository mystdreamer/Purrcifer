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

    public float Length => _healthRange.Length;
    public bool Alive => Health > _healthRange.min;
    public float Health
    {
        get => _healthRange.current;
        set
        {
            _healthRange.current = value;
            _healthRange.Validate();
        }
    }
    public float MaxCap
    {
        get => _healthRange.max;
        set => _healthRange.max = value;
    }
    public float MinCap
    {
        get => _healthRange.min;
        set => _healthRange.min = value;
    }
    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }
    public float InvincibilityLength
    {
        get => _invincibilityLength;
        set => _invincibilityLength = value;
    }

    public List<HealOverTime> Hots => _hots;
    public List<DamageOverTime> Dots => _dots;

    public void SetHealOverTime(HealOverTime hot) => _hots.Add(hot);
    public void SetDamageOverTime(DamageOverTime dot) => _dots.Add(dot);

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

        if (cleanHots) health.CleanHots();
        if (cleanDots) health.CleanDots();
    }

    public static void ApplyBuffs(ref BossHealth health)
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

        if (cleanHots) health.CleanHots();
        if (cleanDots) health.CleanDots();
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

    public EntityHealth(int min, int max, int current)
    {
        this._healthRange = new Range(current, min, max);
    }
}

public abstract class Entity : RoomObjectBase, IEntityInterface
{
    [SerializeField] protected EntityHealth _health;
    public WorldStateContainer container;

    protected virtual void Awake()
    {
        InitialiseHealth();
    }

    protected virtual void InitialiseHealth()
    {
        // Default implementation, can be overridden in derived classes
        _health = new EntityHealth(0, 100, 100);
    }

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

    public float MaxHealth
    {
        get => _health.MaxCap;
        set => _health.MaxCap = value;
    }

    public HealOverTime SetHOT
    {
        set => _health.SetHealOverTime(value);
    }

    public DamageOverTime SetDOT
    {
        set => _health.SetDamageOverTime(value);
    }

    HealOverTime IEntityInterface.SetHot
    {
        set => _health.SetHealOverTime(value);
    }

    DamageOverTime IEntityInterface.SetDot
    {
        set => _health.SetDamageOverTime(value);
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        container.CurrentState = state;
        SetWorldState(state);
    }

    private IEnumerator InvincibilityTimer()
    {
        _health.Invincible = true;
        yield return new WaitForSeconds(_health.InvincibilityLength);
        _health.Invincible = false;
    }

    internal void FillHealth()
    {
        CurrentHealth = MaxHealth;
    }

    internal void UpdateDots() => EntityHealth.ApplyBuffs(ref _health);

    internal abstract void SetWorldState(WorldState state);
    internal abstract void HealthChangedEvent(float lastValue, float currentValue);
    internal abstract void OnDeathEvent();
    internal abstract void InvincibilityActivated();
}