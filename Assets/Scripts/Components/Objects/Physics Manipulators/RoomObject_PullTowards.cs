using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;

public class RoomObject_PullTowards : RoomObjectBase
{
    private bool _pushActive = false;
    public PhysFxPullTowardsPoint pullTowards;


    private void Update()
    {
        if (_pushActive)
            pullTowards.ApplyEffect();
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
