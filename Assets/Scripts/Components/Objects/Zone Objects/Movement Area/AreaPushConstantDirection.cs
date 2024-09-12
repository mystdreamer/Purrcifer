using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class AreaPushConstantDirection : InZone
{
    public ObjectPhysFXPushDirection direction;

    internal override void Update()
    {
        base.Update();

        if (IsActive)
            direction.ApplyEffect(); 
    }

    internal override void OnEnterZone() { }

    internal override void OnExitZone() { }

    internal override void SetWorldState(WorldState state)
    {
        //Apply world state changes. 
    }
}
