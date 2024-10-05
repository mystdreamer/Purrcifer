using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    /// <summary>
    /// The current spawned item. 
    /// </summary>
    public GameObject itemSpawned;

    void Start()
    {
        /////Get an item type to spawn. 
        //GameObject itemPrefab = MasterTree.GetBossPrefab;

        ////Spawn the item and update its position. 
        //itemSpawned = GameObject.Instantiate(itemPrefab);
        //itemSpawned.transform.position = transform.position;
    }
}
