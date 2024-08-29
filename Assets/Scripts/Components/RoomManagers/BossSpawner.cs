using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject itemSpawned;

    void Start()
    {
        GameObject itemPrefab = MasterPool.Instance.BossPrefabTree.GetRandomPrefab(false);
        itemSpawned = GameObject.Instantiate(itemPrefab);
        itemSpawned.transform.position = transform.position;
    }

    void Update()
    {

    }
}
