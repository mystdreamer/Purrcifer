using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 2;       // Max enemy hp
    public int currentHealth;       // Current enemy hp
    public int damageAmount = 1;    // Enemy damage amount

    void Awake()
    {
        // Initialize enemy health
        currentHealth = maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.Player)
        {
            // Access player health from GameManager and apply damage
            GameManager.Instance.PlayerState.Health -= damageAmount;
            Debug.Log("Player takes damage: " + damageAmount);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the enemy collides with an object in the "Weapon" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // Retrieve player stats from the GameManager
            float playerBaseDamage = GameManager.Instance.PlayerState.Damage;

            // Calculate total damage
            float damage = playerBaseDamage;

            // Enemy takes damage
            TakeDamage(damage);
        }
    }

    // damage logic
    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        Debug.Log("Enemy takes damage: " + damage + ". Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // death logic
    private void Die()
    {
        Debug.Log("Enemy has died.");
        // TODO: add more logic here, ie dropping loot, death animations, etc.
        Destroy(gameObject);
    }
}
