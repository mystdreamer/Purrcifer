using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : RoomObjectBase
{
    public Boss bossRef;

    internal override void OnAwakeObject()
    {
        bossRef.gameObject.SetActive(true);
    }

    internal override void OnSleepObject()
    {
        bossRef.gameObject.SetActive(false);
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {

    }

    public void Update()
    {
        if (!bossRef.IsAlive)
            ObjectComplete = true;
    }
}
