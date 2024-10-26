using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class AreaPushConstantDirection : InZone
{
    public ObjectPhysFXPushDirection direction;

    internal override void WorldUpdateReceiver(WorldState state)
    {
        //Apply world state changes. 
    }

    internal override void UpdateObject()
    {
        if (IsActive)
            direction.ApplyEffect();
    }
}
