using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossHealth : EntityHealth
{
    public bool DamageLock
    {
        get;
        set;
    } = false;

    public bool HealLock
    {
        get; set;
    } = false;

    public new float Health
    {
        get => base.Health;

        set
        {
            //Check if the applied value is less than the current and return if locked. 
            if (DamageLock && HealLock && value < base.Health)
                return;

            base.Health = value;
        }
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

    public HealOverTime SetHOT
    {
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

    internal abstract void IncomingDamageDisabled();
    internal abstract void IncomingDamageEnabled();
    #endregion
}
