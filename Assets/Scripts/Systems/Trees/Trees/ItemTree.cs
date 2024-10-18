using UnityEngine;
using ItemPool;
using JetBrains.Annotations;

/// <summary>
/// Component class handling an in game instance of an item tree. 
/// </summary>
[System.Serializable]
public class ItemTree
{
    /// <summary>
    /// The Scriptable object used to define the pools contents. 
    /// </summary>
    public ItemDataSOTree pool;

    /// <summary>
    /// The pools tree.
    /// </summary>
    public ItemBBST poolTree;

    public ItemTree(ItemDataSOTree pool)
    {
        this.pool = pool;
        this.poolTree = new ItemBBST(pool);
    }
}
