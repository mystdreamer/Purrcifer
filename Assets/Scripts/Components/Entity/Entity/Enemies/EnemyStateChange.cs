using UnityEngine;
using Purrcifer.Data.Defaults;

public class EnemyStateChange : MonoBehaviour
{
    [SerializeField] private GameObject childWorldStart;
    [SerializeField] private GameObject childWorldWitching;
    [SerializeField] private GameObject childWorldHell;

    private void Start()
    {
        // Subscribe to the WorldStateChange event to listen for world state changes
        GameManager.Instance.WorldStateChange += OnWorldStateChange;

        // Update the child object based on the current world state
        OnWorldStateChange(GameManager.WorldClock.CurrentState);
    }


    private void OnDestroy()
    {
        // Unsubscribe from the WorldStateChange event when the enemy is destroyed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange -= OnWorldStateChange;
        }
    }

    private void OnWorldStateChange(WorldState newState)
    {
        UpdateChildBasedOnWorldState(newState);
    }

    // Activates the correct child object and deactivates the others based on the world state
    private void UpdateChildBasedOnWorldState(WorldState state)
    {
        childWorldStart.SetActive(false);
        childWorldWitching.SetActive(false);
        childWorldHell.SetActive(false);

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
}
