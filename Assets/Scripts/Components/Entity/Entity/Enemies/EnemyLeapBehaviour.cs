using Purrcifer.Data.Defaults;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyLeapBehaviour : Entity
{
    public enum State
    {
        INACTIVE,
        FOLLOWING,
        CHARGING,
        LEAPING,
        COOLDOWN
    }

    public State enemyState = State.INACTIVE;
    public GameObject playerInstance;
    public NavMeshAgent enemy;
    public Vector3 shownPosition;
    public ParticleSystem stoppingEffect;

    [Header("Leap Settings")]
    public float leapDistance = 5f;
    public float leapSpeed = 10f;
    public float chargeDuration = 2f;
    public float cooldownDuration = 2f;

    private Coroutine currentCoroutine;
    private bool isEffectActive = false;
    private Vector3 leapTarget;
    private Vector3 leapStartPosition;
    private LayerMask wallLayer;

    void Awake()
    {
        shownPosition = transform.position;
        enemy = GetComponent<NavMeshAgent>();
        playerInstance = GameManager.Instance.Player;
        enemy.enabled = false;

        // Get the Wall layer mask
        wallLayer = LayerMask.GetMask("Wall");
    }

    void Update()
    {
        if (enemyState == State.INACTIVE) return;

        transform.LookAt(playerInstance.transform.position);

        switch (enemyState)
        {
            case State.FOLLOWING:
                FollowPlayer();
                break;
            case State.LEAPING:
                PerformLeap();
                break;
        }
    }

    private void FollowPlayer()
    {
        enemy.enabled = true;
        enemy.SetDestination(playerInstance.transform.position);

        if (!enemy.pathPending && enemy.remainingDistance <= enemy.stoppingDistance)
        {
            StartCharging();
        }
    }

    private void StartCharging()
    {
        enemyState = State.CHARGING;
        enemy.enabled = false;

        if (stoppingEffect != null && !isEffectActive)
        {
            stoppingEffect.Play();
            isEffectActive = true;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ChargeAndLeap());
    }

    private Vector3 CalculateLeapTarget()
    {
        Vector3 directionToPlayer = (playerInstance.transform.position - transform.position).normalized;
        Vector3 targetPosition = transform.position + directionToPlayer * leapDistance;

        // Check for walls using raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, leapDistance, wallLayer))
        {
            // If we hit a wall, set the target position slightly before the hit point
            targetPosition = hit.point - (directionToPlayer * 0.5f);
        }

        return targetPosition;
    }

    private IEnumerator ChargeAndLeap()
    {
        yield return new WaitForSeconds(chargeDuration);

        leapStartPosition = transform.position;
        leapTarget = CalculateLeapTarget();

        if (stoppingEffect != null)
        {
            stoppingEffect.Stop();
            isEffectActive = false;
        }

        enemyState = State.LEAPING;

        yield return StartCoroutine(CooldownAfterLeap());
    }

    private void PerformLeap()
    {
        Vector3 directionToTarget = (leapTarget - transform.position).normalized;
        float stepDistance = leapSpeed * Time.deltaTime;
        Vector3 nextPosition = transform.position + directionToTarget * stepDistance;

        // Check for walls in the path of this frame's movement
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, stepDistance, wallLayer))
        {
            // If we hit a wall, stop at the hit point
            transform.position = hit.point - (directionToTarget * 0.5f);
            enemyState = State.COOLDOWN;
            return;
        }

        // If no wall collision, perform the movement
        transform.position = nextPosition;

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, leapTarget) < 0.1f)
        {
            enemyState = State.COOLDOWN;
        }
    }

    private IEnumerator CooldownAfterLeap()
    {
        while (enemyState == State.LEAPING)
        {
            yield return null;
        }

        yield return new WaitForSeconds(cooldownDuration);
        enemyState = State.FOLLOWING;
    }

    internal override void OnAwakeObject()
    {
        shownPosition = gameObject.transform.position;
        playerInstance = GameManager.Instance.Player;
        enemyState = State.FOLLOWING;
    }

    internal override void OnSleepObject()
    {
        enemyState = State.INACTIVE;
        enemy.enabled = false;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        if (stoppingEffect != null && stoppingEffect.isPlaying)
        {
            stoppingEffect.Stop();
            isEffectActive = false;
        }
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue) { }

    internal override void OnDeathEvent()
    {
        enemy.enabled = false;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        gameObject.SetActive(false);
    }

    internal override void InvincibilityActivated() { }

    internal override void SetWorldState(WorldState state) { }

    private void OnDrawGizmos()
    {
        if (enemyState == State.LEAPING)
        {
            // Draw leap path
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leapStartPosition, leapTarget);
            Gizmos.DrawWireSphere(leapTarget, 0.5f);
        }
    }
}