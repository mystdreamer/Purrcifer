using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemPool
{
    /// <summary>
    /// Class used to handle item pools. 
    /// </summary>
    [System.Serializable]
    public class ItemList<T>
    {
        public List<Node<T>> nodes = new List<Node<T>>();

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

        public ItemList() { }

        public ItemList(T t, int key, int weight) =>
            Insert(new Node<T>(t, key, weight));

        private void RegenWeighting()
        {
            List<int> newWeighting = new List<int>();

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].weight; j++)
                {
                    newWeighting.Add(nodes[i].key);
                }
            }

            weightedKeys = newWeighting;
        }

        /// <summary>
        /// Returns a randomly selected item from within the tree. 
        /// </summary>
        /// <param name="remove"> Should the item be removed from the tree upon retrieval. </param>
        /// <returns> GameObject Prefab reference. </returns>
        public T GetRandomPrefab(bool remove)
        {
            int rand = UnityEngine.Random.Range(0, weightedKeys.Count);
            rand = weightedKeys[rand];
            Node<T> val = Search(rand);

            if (val != null)
            {
                if (remove)
                    Delete(rand);

                return val.item;
            }

            return default;
        }

        public bool Insert(Node<T> t)
        {
            nodes.Add(t);
            keys.Add(t.key);
            RegenWeighting();
            SortAndClean();
            return true;
        }

        public void InsertRange(Node<T>[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                this.nodes.Add(nodes[i]);
                keys.Add(nodes[i].key);
            }

            RegenWeighting();
            SortAndClean();
        }

        /// <summary>
        /// Searches and retrieves a Node from within using the provided key.
        /// </summary>
        /// <param name="key"> The ID to search. </param>
        /// <returns> Node instance if found, or NULL if not found. </returns>
        public Node<T> Search(int key)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].key == key)
                    return nodes[i];
            }

            return null;
        }

        /// <summary>
        /// Remove a given node from the tree.
        /// </summary>
        /// <param name="key"> The node to remove. </param>
        public void Delete(int key)
        {
            nodes.RemoveAt(key);
            SortAndClean();
        }

        public void SortAndClean()
        {
            nodes = nodes.Where(x => x != null).ToList();
            nodes = nodes.OrderBy(x => x.key).ToList();
        }
    }
}