using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class Area_PullTowardsPoint : ZoneObject
{
    public ObjectPhysFxPullTowardsPoint pullTowards;

    internal override void WorldUpdateReceiver(WorldState state)
    {

    }

    internal override void UpdateObject()
    {
        if (base.IsActive)
            pullTowards.ApplyEffect();
    }
}