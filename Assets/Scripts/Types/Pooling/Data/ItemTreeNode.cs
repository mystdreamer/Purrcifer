namespace ItemPool
{
    /// <summary>
    /// Node used for defining objects in object pooling data. 
    /// </summary>
    [System.Serializable]
    public class ItemTreeNode
    {
        public static readonly ItemTreeNode NULL_NODE = null;

        public int key;
        public TreeItemData data;
        public ItemTreeNode _left = null;
        public ItemTreeNode _right = null;

        public ItemTreeNode Left { get => _left; set => _left = value; }
        public ItemTreeNode Right { get => _right; set => _right = value; }

        public ItemTreeNode(TreeItemData data)
        {
            this.key = data.key;
            this.data = data;
        }

        public static explicit operator ItemTreeNode(TreeItemData data)
        {
            return new ItemTreeNode(data);
        }

        public string AsString()
        {
            return "[Node-> Name: " + data.objectPrefab.name + ", index: " + key + "]";
        }
    }
}


