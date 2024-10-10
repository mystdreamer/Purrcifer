using Purrcifer.Data.Defaults;
using UnityEngine;

public class TestOnAwakeOnDisable : Entity
{
    public GameObject toEnable; 

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {

    }

    internal override void InvincibilityActivated()
    {
    }

    internal override void OnAwakeObject()
    {
        Debug.Log("Enabled");
        toEnable.SetActive(true);
    }

    internal override void OnDeathEvent()
    {
    }

    internal override void OnSleepObject()
    {
        Debug.Log("Disabled");
        toEnable.SetActive(false);
    }

    internal override void SetWorldState(WorldState state)
    {
    }
}
