using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : RoomObjectBase
{
    public GameObject bossToEnable;

    internal override void OnAwakeObject()
    {
        bossToEnable.SetActive(true);
    }

    internal override void OnSleepObject()
    {
        bossToEnable.SetActive(false);
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {

    }
}
