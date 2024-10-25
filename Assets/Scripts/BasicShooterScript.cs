using UnityEngine;

public class BasicShooterScript : MonoBehaviour
{
    [Header("Target")]
    private GameObject playerInstance;

    [Header("Shooting Configuration")]
    public GameObject projectilePrefab;
    public float shootingRange = 15f;
    public float shootingInterval = 1.5f;
    public float projectileSpeed = 20f;

    [Header("Projectile Spawn Settings")]
    public float spawnOffset = 1f;  // How far in front of the enemy to spawn projectile
    public float spawnHeightOffset = 0.5f;  // How high above the enemy to spawn projectile

    private float nextShootTime = 0f;

    private void Start()
    {
        playerInstance = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (playerInstance == null)
        {
            playerInstance = GameManager.Instance.Player;
            return;
        }

        // Always look at player
        transform.LookAt(playerInstance.transform.position);

        // Check if it's time to shoot and player is in range
        if (Time.time >= nextShootTime && IsPlayerInRange())
        {
            ShootAtPlayer();
            nextShootTime = Time.time + shootingInterval;
        }
    }

    private bool IsPlayerInRange()
    {
        if (playerInstance == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerInstance.transform.position);
        return distanceToPlayer <= shootingRange;
    }

    private void ShootAtPlayer()
    {
        if (projectilePrefab == null || playerInstance == null) return;

        // Calculate direction to player
        Vector3 directionToPlayer = (playerInstance.transform.position - transform.position).normalized;

        // Calculate spawn position
        Vector3 spawnPosition = transform.position +
                              (directionToPlayer * spawnOffset) +
                              (Vector3.up * spawnHeightOffset);

        // Create the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(directionToPlayer));

        // Handle projectile movement
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Physics-based movement
            rb.linearVelocity = directionToPlayer * projectileSpeed;
        }
        else
        {
            // If no Rigidbody, attach a simple mover script
            ProjectileMover mover = projectile.AddComponent<ProjectileMover>();
            mover.direction = directionToPlayer;
            mover.speed = projectileSpeed;
        }

        // Cleanup
        Destroy(projectile, 5f);
    }
}

// Simple projectile movement script for non-rigidbody projectiles
public class ProjectileMover : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}