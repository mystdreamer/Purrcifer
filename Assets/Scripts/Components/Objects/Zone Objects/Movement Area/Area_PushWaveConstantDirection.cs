using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

[RequireComponent(typeof(ObjectEventTicker))]
public class Area_PushWaveConstantDirection : InZone
{
    public ObjectEventTicker objectEventTicker;
    public ObjectPhysFXPushDirection direction;

    internal override void Start()
    {
        base.Start();
        objectEventTicker.Enable = true;
    }

    internal override void Update()
    {
        base.Update();

        if (IsActive && objectEventTicker.TickComplete)
        {
            //Apply forces to the object. 
            objectEventTicker.TickComplete = false;
            direction.ApplyEffect();
        }
    }

    internal override void OnEnterZone()
    {
        objectEventTicker.Enable = true;
    }

    internal override void OnExitZone()
    {
        objectEventTicker.Enable = false;
    }

    internal override void SetWorldState(WorldState state)
    {
        //Apply world state changes. 
    }
}