using UnityEngine;
using Purrcifer.Data.Defaults;

public class EnemyStateChange : MonoBehaviour
{
    [SerializeField] private GameObject childWorldStart;
    [SerializeField] private GameObject childWorldWitching;
    [SerializeField] private GameObject childWorldHell;

    private GameObject currentActiveChild;
    private Vector3 lastActivePosition;

    private void Start()
    {
        GameManager.Instance.WorldStateChange += OnWorldStateChange;
        // Initialize position tracking with current position
        lastActivePosition = GetCurrentActiveChildPosition();
        OnWorldStateChange(GameManager.WorldClock.CurrentState);
    }

    private void OnDestroy()
    {
        ActivateAllChildren();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange -= OnWorldStateChange;
        }
    }

    private Vector3 GetCurrentActiveChildPosition()
    {
        if (childWorldStart.activeSelf) return childWorldStart.transform.position;
        if (childWorldWitching.activeSelf) return childWorldWitching.transform.position;
        if (childWorldHell.activeSelf) return childWorldHell.transform.position;
        return transform.position; // Default to parent position if no child is active
    }

    private void SaveCurrentPosition()
    {
        lastActivePosition = GetCurrentActiveChildPosition();
    }

    private void UpdateChildBasedOnWorldState(WorldState state)
    {
        // Save the position of currently active child before deactivating
        SaveCurrentPosition();

        // Deactivate all child objects first
        childWorldStart.SetActive(false);
        childWorldWitching.SetActive(false);
        childWorldHell.SetActive(false);

        // Activate the appropriate child object and set its position
        GameObject nextActiveChild = null;
        switch (state)
        {
            case WorldState.WORLD_START:
                childWorldStart.SetActive(true);
                nextActiveChild = childWorldStart;
                break;

            case WorldState.WORLD_WITCHING:
                childWorldWitching.SetActive(true);
                nextActiveChild = childWorldWitching;
                break;

            case WorldState.WORLD_HELL:
                childWorldHell.SetActive(true);
                nextActiveChild = childWorldHell;
                break;
        }

        // Update the position of the newly activated child
        if (nextActiveChild != null)
        {
            nextActiveChild.transform.position = lastActivePosition;
            currentActiveChild = nextActiveChild;
        }
    }

    private void OnWorldStateChange(WorldState newState)
    {
        UpdateChildBasedOnWorldState(newState);
    }

    private void ActivateAllChildren()
    {
        childWorldStart.SetActive(true);
        childWorldWitching.SetActive(true);
        childWorldHell.SetActive(true);
    }

    public void DestroyWithChildren()
    {
        ActivateAllChildren();
        Destroy(gameObject);
    }
}