﻿using Purrcifer.Data.Defaults;
using Purrcifer.Object.PhysicsManipulators;
using UnityEngine;

public class Area_PullTowardsPoint : InZone
{
    public PhysFxPullTowardsPoint pullTowards;

    internal override void Update()
    {
        base.Update();

        if (IsActive)
        {
            Debug.Log("Object is active: Applying Pull. ");
            pullTowards.ApplyEffect();
        }
    }

    internal override void OnEnterZone() { }

    internal override void OnExitZone() { }

    internal override void SetWorldState(WorldStateEnum state)
    {
        //Apply world state changes. 
    }
}