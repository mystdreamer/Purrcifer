using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : RoomObjectBase
{
    public Boss bossRef;
    public GameObject trophyPrefab;

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
        if (!bossRef.IsAlive && !ObjectComplete)
        {
            GameObject trophy = GameObject.Instantiate(trophyPrefab);
            trophy.transform.position = new Vector3(Camera.main.transform.position.x, 1, Camera.main.transform.position.z); 
            trophy.SetActive(true);
            ObjectComplete = true;
        }
    }
}
