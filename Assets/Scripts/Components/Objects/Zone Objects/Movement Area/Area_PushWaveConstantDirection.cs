using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

[RequireComponent(typeof(ObjectEventTicker))]
public class Area_PushWaveConstantDirection : InZone
{
    public ObjectEventTicker objectEventTicker;
    public ObjectPhysFXPushDirection direction;

    private void OnEnable()
    {
        Ticker = objectEventTicker;
        objectEventTicker.Enable = true;   
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        //Apply world state changes. 
    }

    internal override void UpdateObject()
    {
        if (IsActive && objectEventTicker.TickComplete)
        {
            //Apply forces to the object. 
            objectEventTicker.TickComplete = false;
            direction.ApplyEffect();
        }
    }
}