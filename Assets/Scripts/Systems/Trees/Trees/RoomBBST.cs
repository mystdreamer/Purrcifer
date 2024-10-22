using System.Collections.Generic;
using UnityEngine;

namespace ItemPool
{
    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class RoomBBST
    {
        public BBST<RoomDataSO> bbst = new BBST<RoomDataSO>();

        /// <summary>
        /// Returns a list of the keys actively held in the tree. 
        /// </summary>
        public List<int> Keys => bbst.Keys;

        #region CTORS. 
        public RoomBBST() { }

        public RoomBBST(RoomDataSO value)
        {
            bbst.Insert(new Node<RoomDataSO>(value, value.roomID, value.weighting));
        }

        public RoomBBST(RoomDataSO[] roomPool)
        {
            InsertRange(roomPool);
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
            RoomDataSO val = bbst.GetRandomPrefab(remove);
            return (val != null) ? val.roomPrefab : null;
        }
        #endregion

        #region Insertion.
        public bool Insert(RoomDataSO data) => 
            bbst.Insert((Node<RoomDataSO>)data);

        public void InsertRange(RoomDataSO[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
                bbst.Insert((Node<RoomDataSO>)nodes[i]);
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