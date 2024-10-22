using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemPool
{
    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class ItemBBST
    {
        public ItemList<ItemDataSO> bbst = new ItemList<ItemDataSO>();

        /// <summary>
        /// Returns a list of the keys actively held in the tree. 
        /// </summary>
        public List<int> Keys => bbst.Keys;

        #region CTORS. 
        public ItemBBST() { }

        public ItemBBST(ItemDataSO value) 
            => bbst.Insert(new Node<ItemDataSO>(value, value.itemID, value.itemWeight));

        public ItemBBST(ItemDataSOTree itemPool) 
            => InsertRange(itemPool.items);
        #endregion

        #region Item Retrival. 

        /// <summary>
        /// Returns a randomly selected item from within the tree. 
        /// </summary>
        /// <param name="remove"> Should the item be removed from the tree upon retrieval. </param>
        /// <returns> GameObject Prefab reference. </returns>
        public GameObject GetRandomPrefab(bool remove) =>
            bbst.GetRandomPrefab(remove)?.powerupPedistoolPrefab;

        public ItemDataSO GetNode(bool remove)
        {
            return bbst.GetRandomNode(remove)?.item;
        }
        #endregion

        #region Insertion.
        public bool Insert(ItemDataSO data) => 
            bbst.Insert((Node<ItemDataSO>)data);

        public void InsertRange(ItemDataSO[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
                bbst.Insert((Node<ItemDataSO>)nodes[i]);
        }
        #endregion

        #region Deletion. 

        /// <summary>
        /// Remove a given node from the tree.
        /// </summary>
        /// <param name="key"> The node to remove. </param>
        public void Delete(int key) => bbst.Delete(key);
        #endregion
    }
}