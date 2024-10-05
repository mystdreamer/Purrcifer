using System.Collections.Generic;
using UnityEngine;

namespace ItemPool
{
    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class ItemBBST
    {
        public BBST<TreeItemDataSO> bbst = new BBST<TreeItemDataSO>();

        /// <summary>
        /// Returns a list of the keys actively held in the tree. 
        /// </summary>
        public List<int> Keys => bbst.Keys;

        #region CTORS. 
        public ItemBBST() { }

        public ItemBBST(TreeItemDataSO value) {
            bbst.Insert(new Node<TreeItemDataSO>(value, value.key, value.probabilityWeight));
        }

        public ItemBBST(ItemTreeSO itemPool)
        {
            InsertRange(itemPool.itemData.ToArray());
        }
        #endregion

        #region Item Retrival. 

        /// <summary>
        /// Returns a randomly selected item from within the tree. 
        /// </summary>
        /// <param name="remove"> Should the item be removed from the tree upon retrieval. </param>
        /// <returns> GameObject Prefab reference. </returns>
        public GameObject GetRandomPrefab(bool remove)
        {
            TreeItemDataSO val = bbst.GetRandomPrefab(remove);
            return val?.objectPrefab;
        }
        #endregion

        #region Insertion.
        public bool Insert(TreeItemDataSO data) => bbst.Insert(new Node<TreeItemDataSO>(data, data.key, data.probabilityWeight));

        public void InsertRange(TreeItemDataSO[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
                Insert(nodes[i]);
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