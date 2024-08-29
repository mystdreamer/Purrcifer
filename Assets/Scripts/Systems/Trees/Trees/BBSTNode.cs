namespace ItemPool
{
    /// <summary>
    /// Node used for defining objects in object pooling data. 
    /// </summary>
    [System.Serializable]
    public class BBSTNode
    {
        public static readonly BBSTNode NULL_NODE = null;

        public int key;
        public ScriptableObj_TreeItemData data;
        public BBSTNode _left = null;
        public BBSTNode _right = null;

        public BBSTNode Left { get => _left; set => _left = value; }
        public BBSTNode Right { get => _right; set => _right = value; }

        public BBSTNode(ScriptableObj_TreeItemData data)
        {
            this.key = data.key;
            this.data = data;
        }

        public static explicit operator BBSTNode(ScriptableObj_TreeItemData data)
        {
            return new BBSTNode(data);
        }

        public string AsString()
        {
            return "[Node-> Name: " + data.objectPrefab.name + ", index: " + key + "]";
        }
    }
}


