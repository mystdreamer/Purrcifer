using System.Collections.Generic;
using UnityEngine;

namespace ItemPool
{

    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class UpgradeBBST
    {
        public BBST<ItemDataSO> bbst = new BBST<ItemDataSO>();

        /// <summary>
        /// Returns a list of the keys actively held in the tree. 
        /// </summary>
        public List<int> Keys => bbst.Keys;

        #region CTORS. 
        public UpgradeBBST() { }

        public UpgradeBBST(ItemDataSO value)
        {
            bbst.Insert(new Node<ItemDataSO>(value, value.itemID, value.itemWeight));
        }

        public UpgradeBBST(ItemDataSOTree itemPool)
        {
            InsertRange(itemPool.items);
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
            ItemDataSO val = bbst.GetRandomPrefab(remove);
            
            if (val != null)
                return val.powerupPedistoolPrefab;
            else
                return null;
        }
        #endregion

        #region Insertion.
        public bool Insert(Node<ItemDataSO> node) => bbst.Insert(node);

        public void InsertRange(ItemDataSO[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
                bbst.Insert(new Node<ItemDataSO>(nodes[i], nodes[i].itemID, nodes[i].itemWeight));
        }
        #endregion

        #region Search. 

        /// <summary>
        /// Searches and retrieves a Node from within using the provided key.
        /// </summary>
        /// <param name="key"> The ID to search. </param>
        /// <returns> Node instance if found, or NULL if not found. </returns>
        public Node<ItemDataSO> Search(int key) => bbst.Search(key);

        #endregion

        /// <summary>
        /// Remove a given node from the tree.
        /// </summary>
        /// <param name="targetKey"> The node to remove. </param>
        public void Delete(int targetKey) => bbst.Delete(targetKey);
    }
}