using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class Area_PullTowardsPoint : RoomObjectBase
{
    public ObjectPhysFxPullTowardsPoint pullTowards;
    bool isActive = false;

    internal override void OnAwakeObject()
    {
        isActive = true;
    }

    internal override void OnSleepObject()
    {
        isActive = false;
    }

    private void Update()
    {
        if (isActive)
            pullTowards.ApplyEffect();
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {

    }
}