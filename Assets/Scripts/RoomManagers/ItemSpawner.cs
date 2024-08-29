using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemSpawnPoint;
    public GameObject itemSpawned; 

    void Start()
    {
        GameObject itemPrefab = MasterPool.Instance.ItemTree.GetRandomPrefab(false);
        itemSpawned = GameObject.Instantiate(itemPrefab);
        itemSpawned.transform.position = itemSpawnPoint.transform.position;
    }

    void Update()
    {
        
    }
}
