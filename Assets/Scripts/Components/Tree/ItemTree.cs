using UnityEngine;
using ItemPool;
using JetBrains.Annotations;

/// <summary>
/// Component class handling an in game instance of an item tree. 
/// </summary>
public class ItemTree : MonoBehaviour
{
    /// <summary>
    /// The Scriptable object used to define the pools contents. 
    /// </summary>
    public ItemTreeSO pool;

    /// <summary>
    /// The pools tree.
    /// </summary>
    public ItemBBST poolTree;

    private void Awake()
    {
        this.poolTree = new ItemBBST(pool);
    }

    /// <summary>
    /// Used to spawn an item from the pool. 
    /// </summary>
    /// <param name="spawnPos"> The position to spawn the item at. </param>
    /// <param name="delete"> Should the prefab be deleted upon spawn. </param>
    /// <returns> reference to the new instance generated.  </returns>
    public GameObject SpawnItem(Vector2 spawnPos, bool delete)
    {
        GameObject prefab = poolTree.GetRandomPrefab(delete);
        GameObject spawn = GameObject.Instantiate(prefab);
        spawn.transform.position = spawnPos;
        return spawn;
    }
}
