using Purrcifer.Data.Defaults;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootProjectile : Entity
{
    public State enemyState = State.INACTIVE;
    public GameObject playerInstance;
    public NavMeshAgent enemy;
    public Vector3 shownPosition;

    [Header("Shooting Configuration")]
    public GameObject projectilePrefab;  // Assign in inspector
    public float shootingRange = 10f;    // Maximum range to shoot
    public float shootingInterval = 2f;  // Time between shots
    private float nextShootTime = 0f;    // Internal timer for shooting

    public enum State
    {
        INACTIVE,
        ACTIVE
    }

    void Awake()
    {
        shownPosition = transform.position;
        enemy = GetComponent<NavMeshAgent>();
        playerInstance = GameManager.Instance.Player;
        // Initially disable the NavMeshAgent
        enemy.enabled = false;
    }

    void Update()
    {
        if (enemyState == State.ACTIVE)
        {
            transform.LookAt(playerInstance.transform.position);
            enemy.SetDestination(playerInstance.transform.position);

            // Check if it's time to shoot and if player is in range
            if (Time.time >= nextShootTime && CanShootPlayer())
            {
                ShootProjectile();
                nextShootTime = Time.time + shootingInterval;
            }
        }
    }

    private bool CanShootPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerInstance.transform.position);
        return distanceToPlayer <= shootingRange;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            // Calculate direction to player
            Vector3 directionToPlayer = (playerInstance.transform.position - transform.position).normalized;

            // Spawn projectile slightly in front of the enemy
            Vector3 spawnPosition = transform.position + directionToPlayer + Vector3.up * 0.5f;

            // Instantiate and orient the projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // If the projectile has a Rigidbody, apply force
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                float projectileSpeed = 20f; // Adjust this value as needed
                projectileRb.linearVelocity = directionToPlayer * projectileSpeed;
            }

            // Optional: Destroy projectile after some time if it doesn't hit anything
            Destroy(projectile, 5f);
        }
    }

    internal override void OnAwakeObject()
    {
        shownPosition = gameObject.transform.position;
        playerInstance = GameManager.Instance.Player;

        // Set enemy state to active and enable NavMeshAgent
        enemyState = State.ACTIVE;
        enemy.enabled = true;

        // Debug.Log("Enemy is active and NavMeshAgent enabled.");
    }

    internal override void OnSleepObject()
    {
        // Set enemy state to inactive and disable NavMeshAgent
        enemyState = State.INACTIVE;
        enemy.enabled = false;

        // Debug.Log("Enemy is inactive and NavMeshAgent disabled.");
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue) { }

    internal override void OnDeathEvent()
    {
        // Optionally disable the NavMeshAgent when the enemy dies
        enemy.enabled = false;
        gameObject.SetActive(false);
    }

    internal override void InvincibilityActivated() { }

    internal override void SetWorldState(WorldState state) { }
}