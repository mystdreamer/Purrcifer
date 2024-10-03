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
        INACTIVE,
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
