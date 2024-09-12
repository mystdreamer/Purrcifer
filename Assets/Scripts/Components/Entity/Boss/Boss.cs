using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossHealth : EntityHealth
{
    public bool PreventDamage
    {
        get => _bossDamageLock;
        set => _bossDamageLock = value;
    }

    /// <summary>
    /// CTOR. 
    /// </summary>
    /// <param name="min"> The minimum health value of the player. </param>
    /// <param name="max"> The maximum health value of the player. </param>
    /// <param name="current"> The current health of the player. </param>
    public BossHealth(int min, int max, int current) : base(min, max, current)
    {

    }
}

public abstract class Boss : WorldObject, IEntityInterface
{
    [SerializeField] private BossHealth _health;
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

    public BossHealth BHealth => _health;

    public float CurrentHealth
    {
        get => BHealth.Health;
        set => BHealth.Health = value;
    }

    public float HealthCap
    {
        get => BHealth.MaxCap;
        set => BHealth.MaxCap = value;
    }

    public bool LockHealth
    {
        get => BHealth._bossDamageLock;
        set
        {
            if (value)
                IncomingDamageDisabled();
            else
                IncomingDamageEnabled();

            BHealth._bossDamageLock = value;
            
        }
    }

    #endregion

    public override void WorldUpdateReceiver(WorldState state)
    {
        container.SetState = state;
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

    internal abstract void IncomingDamageDisabled();
    internal abstract void IncomingDamageEnabled();
    #endregion
}
