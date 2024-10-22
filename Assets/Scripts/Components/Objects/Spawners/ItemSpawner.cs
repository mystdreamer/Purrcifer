using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to spawn an item at runtime. 
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    /// <summary>
    /// GameObject providing the location where the item should be spawned.  
    /// </summary>
    [Header("The gameobject providing a point for the object to be spawned at.")]
    public GameObject itemSpawnPoint;

    public ItemDataSO itemDataSO;

    /// <summary>
    /// The current spawned item. 
    /// </summary>
    public GameObject itemSpawned; 

    void Start()
    {
        ///Get an item type to spawn. 
        itemDataSO = MasterTree.Instance.GetItemSpawnTreasureRoomSO;
        //Spawn the item and update its position. 
        GameObject itemSpawned = GameObject.Instantiate(itemDataSO.powerupPedistoolPrefab);
        itemSpawned.transform.position = itemSpawnPoint.transform.position;
    }
}
