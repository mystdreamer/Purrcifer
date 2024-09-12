using Purrcifer.Data.Defaults;
using UnityEngine;

public class BossTestScript : Boss
{
    internal override void ApplyWorldState(WorldState state)
    {
        Debug.Log(state.ToString());
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {

    }

    internal override void IncomingDamageDisabled()
    {

    }

    internal override void IncomingDamageEnabled()
    {

    }

    internal override void InvincibilityActivated()
    {

    }

    internal override void OnDeathEvent()
    {

    }

    void Start()
    {
        UIManager.SetBossHealth(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            ((IEntityInterface)this).Health -= 10;
    }
}
