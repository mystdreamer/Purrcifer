namespace ItemPool
{
    /// <summary>
    /// Node used for defining objects in object pooling data. 
    /// </summary>
    [System.Serializable]
    public class BBSTNode
    {
        public static readonly BBSTNode NULL_NODE = null;

        /// <summary>
        /// The key representing the nodes position.
        /// </summary>
        public int key;
        
        /// <summary>
        /// The scriptable object data reference carried by the node. 
        /// </summary>
        public TreeItemDataSO data;

        /// <summary>
        /// The left leaf. 
        /// </summary>
        public BBSTNode _left = null;

        /// <summary>
        /// The right leaf. 
        /// </summary>
        public BBSTNode _right = null;

        /// <summary>
        /// Returns the left leaf of the node. 
        /// </summary>
        public BBSTNode Left { get => _left; set => _left = value; }

        /// <summary>
        /// Returns the right leaf of the node. 
        /// </summary>
        public BBSTNode Right { get => _right; set => _right = value; }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="data"> The data to be help by the node. </param>
        public BBSTNode(TreeItemDataSO data)
        {
            this.key = data.key;
            this.data = data;
        }

        /// <summary>
        /// Returns a new BBSTNode from a ScriptableObj_TreeItemData instance. 
        /// </summary>
        /// <param name="data"> The ScriptableObj_TreeItemData to convert. </param>
        public static explicit operator BBSTNode(TreeItemDataSO data)
        {
            return new BBSTNode(data);
        }

        /// <summary>
        /// Returns the BBSTNode as a formatted string.  
        /// </summary>
        public string AsString()
        {
            return "[Node-> Name: " + data.objectPrefab.name + ", index: " + key + "]";
        }
    }
}


