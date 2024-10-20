using UnityEngine;
using Purrcifer.Data.Defaults;
using Purrcifer.Entity.HotsDots;

public class Enemy : Entity
{
    [Header("Enemy Stats")]
    public int maxHealth = 10;
    public int damageAmount = 1;

    internal override void OnAwakeObject()
    {
        Debug.Log("Enemy Awake: OnAwakeObject called.");
    }

    internal override void OnSleepObject()
    {
        Debug.Log("Enemy Sleep: OnSleepObject called.");
    }

    new private void Start()
    {
        base.Start();  // Call base Start() to retain inherited behavior
        CurrentHealth = maxHealth;  // Initialize health
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

    // Handle the damage enemy takes
    public void TakeDamage(float damage)
    {
        ((IEntityInterface)this).Health -= damage;
        Debug.Log("Enemy takes damage: " + damage + ". Current Health: " + CurrentHealth);

        if (!((IEntityInterface)this).IsAlive)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {
        // TODO: Add health bar update logic here for enemy UI
        Debug.Log($"Enemy health changed from {lastValue} to {currentValue}");
    }

    internal override void OnDeathEvent()
    {
        //TODO: Death animation, sound, etc.
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
