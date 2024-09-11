using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public interface IRoomObjectEffect
{
    public void ApplyEffect();
}

public class RoomObject_PushConstantDirection : RoomObjectBase
{
    private bool _pushActive = false;
    public PhysFXPushDirection pushDirection;


    private void Update()
    {
        if (_pushActive)
            pushDirection.ApplyEffect();
    }

    internal override void OnAwakeObject()
    {
        _pushActive = true;
    }

    internal override void OnSleepObject()
    {
        _pushActive = false;
    }

    internal override void SetWorldState(WorldStateEnum state)
    {

    }
}
