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
        StartCoroutine(SelectItem());
    }

    IEnumerator SelectItem()
    {
        ItemDataSO selected;
        bool verifiedDrop = false;
        while (!verifiedDrop)
        {
            selected = MasterTree.Instance.GetItemSpawnTreasureRoomSO;
            if (selected != null)
            {
                if (selected.powerupPedistoolPrefab != null)
                {
                    verifiedDrop = true;
                    itemDataSO = selected;

                    //Spawn the item and update its position. 
                    GameObject itemSpawned = GameObject.Instantiate(itemDataSO.powerupPedistoolPrefab);
                    itemSpawned.transform.position = itemSpawnPoint.transform.position;
                    yield return true;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
