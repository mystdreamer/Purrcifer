using Purrcifer.Data.Defaults;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PopwormController : Entity
{
    public enum PopwormState
    {
        INACTIVE,
        UNDERGROUND,
        POPPED
    }

    public PopwormState popwormState = PopwormState.INACTIVE;
    public float revealHeight = 1f;

    public float revealSpeed;
    public float hideSpeed;

    public float detectionRadius = 2f;
    public Vector3 shownPosition;
    public Vector3 hiddenPosition;
    public Vector3 currentPosition;
    public Vector3 directionVec;
    public GameObject playerInstance;
    public bool isChangingState = false;

    public bool PlayerDetected =>
        (Vector3.Distance(transform.position, playerInstance.transform.position) < detectionRadius);

    void Awake()
    {
        shownPosition = transform.position;
        currentPosition = hiddenPosition = shownPosition - Vector3.up * revealHeight;
        gameObject.transform.position = currentPosition;
        directionVec = (shownPosition - hiddenPosition).normalized;
    }

    void Update()
    {
        if (popwormState == PopwormState.INACTIVE || isChangingState)
            return;

        switch (popwormState)
        {
            case PopwormState.UNDERGROUND:
                State_Underground();
                break;
            case PopwormState.POPPED:
                break;
        }
    }

    private void State_Underground()
    {
        //Detect whether the player is close enough for the worm to appear. 
        //If so, reveal it, and the swap state.

        //Get the distance between the player and the worm. 
        if (PlayerDetected)
        {
            Debug.Log("Popworm: Changing AI state -> Popped");
            isChangingState = true;
            Debug.Log("Popworm: Player detected.");
            StartCoroutine(Reveal());
        }
    }

    private void State_PoppedUp()
    {
        //If player leaves the worms radius, then make the worm retract. 
        //Swap state to underground. 
    }

    private IEnumerator Reveal()
    {
        Vector3 stepVector = Vector3.up.normalized;
        float currentTime = 0;

        while (currentTime < revealSpeed - 0.1f)
        {
            currentTime += Time.deltaTime;
            currentPosition = hiddenPosition + (stepVector * currentTime);
            transform.position = currentPosition;
            yield return new WaitForFixedUpdate();
        }

        currentPosition = hiddenPosition + stepVector * revealHeight;
        transform.position = currentPosition;

        popwormState = PopwormState.POPPED;
        isChangingState = false;
    }

    //private IEnumerator Hide()
    //{

    //}

    internal override void OnAwakeObject()
    {
        shownPosition = gameObject.transform.position;
        playerInstance = GameManager.Instance.Player;
        popwormState = PopwormState.UNDERGROUND;
    }

    internal override void OnSleepObject()
    {
        popwormState = PopwormState.INACTIVE;
    }

    private void OnDrawGizmosSelected()
    {
        hiddenPosition = transform.position - new Vector3(0, revealHeight / 2, 0);
        shownPosition = transform.position + new Vector3(0, revealHeight / 2, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hiddenPosition + Vector3.right, 0.05f);
        Gizmos.DrawSphere(shownPosition + Vector3.right, 0.05f);
        Vector3 radiusDrawPos = hiddenPosition + (Vector3.up * revealHeight / 2);
        radiusDrawPos.y = 1;

        Gizmos.DrawWireSphere(radiusDrawPos, detectionRadius);
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
