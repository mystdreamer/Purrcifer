using UnityEngine;
using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;

public class Enemy : Entity
{
    [Header("Enemy Stats")]
    public int maxHealth = 10;
    public int damageAmount = 1;

    protected override void InitializeHealth()
    {
        _health = new EntityHealth(0, maxHealth, maxHealth);
    }

    protected override void Awake()
    {
        base.Awake();  // This will call InitializeHealth
        Debug.Log($"Starting Enemy with health: {CurrentHealth}/{MaxHealth}");
    }

    internal override void OnAwakeObject()
    {
        Debug.Log("Enemy Awake: OnAwakeObject called.");
    }

    internal override void OnSleepObject()
    {
        Debug.Log("Enemy Sleep: OnSleepObject called.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.Player)
        {
            GameManager.Instance.PlayerState.Health -= damageAmount;
            Debug.Log("Player takes damage: " + damageAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            float playerBaseDamage = GameManager.Instance.PlayerState.Damage;
            TakeDamage(playerBaseDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        ((IEntityInterface)this).Health -= damage;
        Debug.Log($"Enemy takes damage: {damage}. Current Health: {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject.transform.parent.gameObject);
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {
        Debug.Log($"Enemy health changed from {lastValue} to {currentValue}");
    }

    internal override void OnDeathEvent()
    {
        ObjectComplete = true;
        Die();
    }

    internal override void InvincibilityActivated()
    {
        Debug.Log("Enemy is invincible for a short time.");
    }

    internal override void SetWorldState(WorldState state)
    {
        Debug.Log("World state updated for enemy.");
    }
}