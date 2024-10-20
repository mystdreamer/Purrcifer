using UnityEngine;
using Purrcifer.Data.Defaults; // Ensure the namespace containing WorldState is imported

public class EnemyStateChange : MonoBehaviour
{
    [SerializeField] private GameObject childWorldStart;
    [SerializeField] private GameObject childWorldWitching;
    [SerializeField] private GameObject childWorldHell;

    private void Start()
    {
        // Subscribe to the WorldStateChange event to listen for world state changes
        GameManager.Instance.WorldStateChange += OnWorldStateChange;

        // Initialize the enemy based on the current world state by accessing WorldClock statically
        OnWorldStateChange(GameManager.WorldClock.CurrentState);
    }

    private void OnDestroy()
    {
        // Ensure all child objects are activated before destruction
        ActivateAllChildren();

        // Unsubscribe from the WorldStateChange event when the enemy is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange -= OnWorldStateChange;
        }
    }

    // Ensure this method matches the WorldUpdateEvent delegate's signature
    private void OnWorldStateChange(WorldState newState)
    {
        UpdateChildBasedOnWorldState(newState);
    }

    // Activates the correct child object and deactivates the others based on the world state
    private void UpdateChildBasedOnWorldState(WorldState state)
    {
        ActivateAllChildren();
        // Deactivate all child objects first
        childWorldStart.SetActive(false);
        childWorldWitching.SetActive(false);
        childWorldHell.SetActive(false);

        // Activate the appropriate child object based on the current state
        switch (state)
        {
            case WorldState.WORLD_START:
                childWorldStart.SetActive(true);
                break;

            case WorldState.WORLD_WITCHING:
                childWorldWitching.SetActive(true);
                break;

            case WorldState.WORLD_HELL:
                childWorldHell.SetActive(true);
                break;
        }
    }

    // Ensure all child objects are activated before the object is destroyed
    private void ActivateAllChildren()
    {
        childWorldStart.SetActive(true);
        childWorldWitching.SetActive(true);
        childWorldHell.SetActive(true);
    }

    // Override Destroy method to ensure all child objects are active before destruction
    public void DestroyWithChildren()
    {
        // Activate all children first
        ActivateAllChildren();

        // Now destroy the object
        Destroy(gameObject);
    }
}
