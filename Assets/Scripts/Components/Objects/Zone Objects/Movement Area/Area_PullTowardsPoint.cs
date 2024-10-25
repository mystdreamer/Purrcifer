using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class Area_PullTowardsPoint : RoomObjectBase
{
    public ObjectPhysFxPullTowardsPoint pullTowards;
    bool isActive = false;

    private void Awake()
    {
        if(activationType == ObjectActivationType.ON_OBJECT_START)
            isActive = true;
    }

    internal override void OnAwakeObject()
    {
        if (activationType == ObjectActivationType.ON_OBJECT_START)
            return;

        isActive = true;
    }

    internal override void OnSleepObject()
    {
        if (activationType == ObjectActivationType.ON_OBJECT_START)
            return;

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