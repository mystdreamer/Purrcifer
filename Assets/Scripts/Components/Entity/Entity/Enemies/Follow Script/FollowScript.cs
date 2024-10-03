using Purrcifer.Data.Defaults;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : Entity
{
    public enum State
    {
        INACTIVE,using Purrcifer.Data.Defaults;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
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

        Debug.Log("Enemy is active and NavMeshAgent enabled.");
    }

    internal override void OnSleepObject()
    {
        // Set enemy state to inactive and disable NavMeshAgent
        enemyState = State.INACTIVE;
        enemy.enabled = false;

        Debug.Log("Enemy is inactive and NavMeshAgent disabled.");
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

ACTIVE
    }
    public State enemyState = State.INACTIVE;
public GameObject playerInstance;
public NavMeshAgent enemy;
public Vector3 shownPosition;

void Awake()
{
    shownPosition = transform.position;
    enemy = GetComponent<NavMeshAgent>();
    playerInstance = GameObject.FindWithTag("Player");
}

void Update()
{
    if (enemyState != State.INACTIVE)
    {
        transform.LookAt(playerInstance.transform.position);
        enemy.SetDestination(playerInstance.transform.position);
    }
}

internal override void OnAwakeObject()
{
    shownPosition = gameObject.transform.position;
    playerInstance = GameManager.Instance.Player;
    enemyState = State.ACTIVE;
}

internal override void OnSleepObject()
{
    enemyState = State.INACTIVE;
}
internal override void HealthChangedEvent(float lastValue, float currentValue) { }

internal override void OnDeathEvent()
{
    gameObject.SetActive(false);
}

internal override void InvincibilityActivated() { }

internal override void SetWorldState(WorldState state)
{

}
}
