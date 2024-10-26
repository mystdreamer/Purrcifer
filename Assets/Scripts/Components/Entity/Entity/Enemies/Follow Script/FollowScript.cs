using Purrcifer.Data.Defaults;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : Entity
{
    public State enemyState = State.INACTIVE;
    public GameObject playerInstance;
    public NavMeshAgent enemy;
    public Vector3 shownPosition;

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
