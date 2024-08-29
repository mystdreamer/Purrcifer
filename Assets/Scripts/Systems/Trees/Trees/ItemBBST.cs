using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ItemPool
{
    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class ItemBBST
    {
        /// <summary>
        /// The current root node in the tree. 
        /// </summary>
        public BBSTNode root = null;

        /// <summary>
        /// The keys held in the tree. 
        /// </summary>
        [SerializeField] private List<int> keys = new List<int>();

        /// <summary>
        /// The keys held in the tree. 
        /// </summary>
        [SerializeField] private List<int> weightedKeys = new List<int>();

        /// <summary>
        /// Returns a list of the keys actively held in the tree. 
        /// </summary>
        public List<int> Keys => keys;

        private int AddID
        {
            set { keys.Add(value); }
        }

        #region Weighting Calculations. 
        private void AddWeighting(BBSTNode node)
        {
            for (int i = 0; i < node.data.probabilityWeight; i++)
                weightedKeys.Add(node.key);
        }

        private void RegenWeighting(int itemID)
        {
            List<int> newWeighting = new List<int>();

            for (int i = 0; i < weightedKeys.Count; i++)
            {
                if (keys[i] != itemID)
                    newWeighting.Add(weightedKeys[i]);
            }

            weightedKeys = newWeighting;
        }
        #endregion

        #region CTORS. 
        public ItemBBST() { }

        public ItemBBST(BBSTNode value) { root = value; }

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
            int rand = UnityEngine.Random.Range(0, weightedKeys.Count);
            rand = weightedKeys[rand];
            BBSTNode val = Search(rand);

            if (remove && val != null)
            {
                Delete(rand);
            }
            if (val != null)
                return val.data.objectPrefab;

            return null;
        }
        #endregion

        #region Insertion.
        public bool Insert(BBSTNode node)
        {
            BBSTNode searchedNode = Search(node.key);
            if (searchedNode != null)
                return false;
            else
            {
                //No root case, insert as root. 
                if (root == null)
                {
                    root = node;

                    //Add the weighted probability. 
                    AddWeighting(node);
                    Heapify(root);
                    return true;
                }
                else
                {
                    bool result = Insert(root, node);
                    //Add the weighted probability. 
                    AddWeighting(node);
                    Heapify(root);
                    return result;
                }
            }
        }

        public void InsertRange(ScriptableObj_TreeItemData[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
                Insert((BBSTNode)nodes[i]);
        }

        private bool Insert(BBSTNode root, BBSTNode node)
        {
            //Root is a duplicate. 
            if (root.key == node.key)
                return false;
            else
            {
                if (root.Left == null && node.key < root.key)
                {
                    root.Left = node;
                    return true;
                }
                else if (root.Right == null && node.key > root.key)
                {
                    root.Right = node;
                    return true;
                }
                else
                {

                    if (root.Left != null)
                    {
                        if (root.Left.key == node.key)
                            return false;
                        else if (root.Left.key > node.key)
                            return Insert(root.Left, node);
                    }

                    if (root.Right != null)
                    {
                        if (root.Right.key == node.key)
                            return false;
                        else if (root.Right.key < node.key)
                            return Insert(root.Right, node);
                    }
                }
            }

            return false;
        }
        #endregion

        #region Search. 

        /// <summary>
        /// Searches and retrieves a Node from within using the provided key.
        /// </summary>
        /// <param name="key"> The ID to search. </param>
        /// <returns> Node instance if found, or NULL if not found. </returns>
        public BBSTNode Search(int key)
        {
            return Search(root, key);
        }

        /// <summary>
        /// Searches and retrieves a Node from within using the provided key.
        /// </summary>
        /// <param name="root"> The root Node to search. </param>
        /// <param name="key"> The ID to search. </param>
        private BBSTNode Search(BBSTNode root, int key)
        {
            if (root == null || root.key == key)
                return root;
            else
            {
                if (root.Left != null)
                {
                    if (root.Left.key == key)
                        return root.Left;
                    else if (root.Left.key > key)
                        return Search(root.Left, key);
                }

                if (root.Right != null)
                {
                    if (root.Right.key == key)
                        return root.Right;
                    else if (root.Right.key < key)
                    {
                        return Search(root.Right, key);
                    }
                }
            }

            return null;
        }

        #endregion

        #region Deletion. 

        /// <summary>
        /// Remove a given node from the tree.
        /// </summary>
        /// <param name="targetKey"> The node to remove. </param>
        public void Delete(int targetKey)
        {
            root = Delete(root, targetKey);
            keys.Remove(targetKey);
            RegenWeighting(targetKey);
        }

        /// <summary>
        /// Remove a given node from the tree. 
        /// </summary>
        /// <param name="node"> The root node to search. </param>
        /// <param name="targetKey"> The key of the node to remove.  </param>
        /// <returns> The node once removed from the tree. </returns>
        private BBSTNode Delete(BBSTNode node, int targetKey)
        {
            BBSTNode parent;
            if (node == null)
            { return null; }
            else
            {
                //left subtree
                if (targetKey < node.key)
                {
                    node.Left = Delete(node.Left, targetKey);
                    if (GetBalance(node) == -2)//here
                    {
                        if (GetBalance(node.Right) <= 0)
                            node = RRRotation(node);
                        else
                            node = RLRotation(node);
                    }
                }
                //right subtree
                else if (targetKey > node.key)
                {
                    node.Right = Delete(node.Right, targetKey);
                    if (GetBalance(node) == 2)
                    {
                        if (GetBalance(node.Left) >= 0)
                            node = LLRotation(node);
                        else
                            node = LRRotation(node);
                    }
                }
                //if target is found
                else
                {
                    if (node.Right != null)
                    {
                        //delete its inorder successor
                        parent = node.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        node.key = parent.key;
                        node.Right = Delete(node.Right, parent.key);
                        if (GetBalance(node) == 2)//rebalancing
                        {
                            if (GetBalance(node.Left) >= 0)
                            {
                                node = LLRotation(node);
                            }
                            else { node = LRRotation(node); }
                        }
                    }
                    else
                    {   //if current.left != null
                        return node.Left;
                    }
                }
            }
            return node;
        }
        #endregion

        #region AVL Sorting. 
        /// <summary>
        /// Rebalances the tree after inserting a node. 
        /// </summary>
        /// <param name="root"> The root node to rebalance. </param>
        /// <returns> a balanced version of the provided Node. </returns>
        private BBSTNode Heapify(BBSTNode root)
        {
            if (root == null)
                return root;

            int balanceFactor = GetBalance(root);

            //Balance the left side of the tree? 
            if (balanceFactor > 0)
            {
                if (GetBalance(root.Left) > 0)
                    root = LLRotation(root);
                else
                    root = LRRotation(root);
            }

            //Balance the right side of the tree? 
            else if (balanceFactor < -1)
            {
                if (GetBalance(root.Right) > 0)
                    root = RLRotation(root);
                else root = RRRotation(root);
            }

            return root;
        }

        /// <summary>
        /// Returns the maximum height of the node's path. 
        /// </summary>
        /// <param name="node"> The node to calculate the height of. </param>
        /// <returns> int representing the height of the node. </returns>
        private int GetHeight(BBSTNode node)
        {
            int height = 0;
            if (node != null)
            {
                int l = (node.Left != null) ? GetHeight(node.Left) : 0;
                int r = (node.Left != null) ? GetHeight(node.Left) : 0;
                int m = Max(l, r);
                height = m + 1;
            }
            return height;
        }

        private int Max(int l, int r)
        {
            return l > r ? l : r;
        }

        /// <summary>
        /// Calculate the balance factor of the BST. 
        /// </summary>
        /// <param name="node"> The node to calculate. </param>
        /// <returns> Int with current balance of the node and its given children. </returns>
        private int GetBalance(BBSTNode node)
        {
            int l = GetHeight(node.Left);
            int r = GetHeight(node.Right);
            int factor = l - r;
            return factor;
        }

        private BBSTNode LLRotation(BBSTNode node)
        {
            BBSTNode newRoot = node.Left;
            newRoot.Left = node.Left.Left;
            newRoot.Right = node;

            return newRoot;
        }

        private BBSTNode RRRotation(BBSTNode node)
        {
            BBSTNode newRoot = node.Right;
            newRoot.Left = node;
            newRoot.Right = node.Right.Right;
            return newRoot;
        }

        private BBSTNode LRRotation(BBSTNode node)
        {
            BBSTNode newRoot = node;
            BBSTNode temp = newRoot.Left;
            newRoot.Left = node.Left.Right;
            newRoot.Left.Left = temp;

            return RRRotation(newRoot);
        }

        private BBSTNode RLRotation(BBSTNode node)
        {
            BBSTNode newRoot = node;
            BBSTNode temp = newRoot.Right;
            newRoot.Right = node.Right.Left;
            newRoot.Right.Right = temp;
            return LLRotation(newRoot);
        }
        #endregion
    }
}