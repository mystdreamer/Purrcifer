using UnityEngine;
using Purrcifer.Data.Defaults;

public class LightStateChange : MonoBehaviour
{
    [SerializeField] private Light lightWorldStart;
    [SerializeField] private Light lightWorldWitching;
    [SerializeField] private Light lightWorldHell;

    private Light currentActiveLight;

    private void Start()
    {
        GameManager.Instance.WorldStateChange += OnWorldStateChange;
        OnWorldStateChange(GameManager.WorldClock.CurrentState);
    }

    private void OnDestroy()
    {
        ActivateAllLights();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange -= OnWorldStateChange;
        }
    }

    private void UpdateLightBasedOnWorldState(WorldState state)
    {
        // Deactivate all lights first
        lightWorldStart.enabled = false;
        lightWorldWitching.enabled = false;
        lightWorldHell.enabled = false;

        // Activate the appropriate light
        Light nextActiveLight = null;
        switch (state)
        {
            case WorldState.WORLD_START:
                lightWorldStart.enabled = true;
                nextActiveLight = lightWorldStart;
                break;

            case WorldState.WORLD_WITCHING:
                lightWorldWitching.enabled = true;
                nextActiveLight = lightWorldWitching;
                break;

            case WorldState.WORLD_HELL:
                lightWorldHell.enabled = true;
                nextActiveLight = lightWorldHell;
                break;
        }

        currentActiveLight = nextActiveLight;
    }

    private void OnWorldStateChange(WorldState newState)
    {
        UpdateLightBasedOnWorldState(newState);
    }

    private void ActivateAllLights()
    {
        lightWorldStart.enabled = true;
        lightWorldWitching.enabled = true;
        lightWorldHell.enabled = true;
    }

    public void DestroyWithLights()
    {
        ActivateAllLights();
        Destroy(gameObject);
    }
}