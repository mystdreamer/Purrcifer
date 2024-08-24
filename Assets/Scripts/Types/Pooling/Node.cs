namespace ItemPool
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

    [System.Serializable ]
    public class Node
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public static readonly Node NULL_NODE = null;

        public int key;
        public PoolItemData data;
        public Node _left = null; 
        public Node _right = null;

        public Node Left { get => _left; set => _left = value; }
        public Node Right { get => _right; set => _right = value; }

        public Node(PoolItemData data)
        {
            this.key = data.key; 
            this.data = data;
        }

        public static explicit operator Node(PoolItemData data)
        {
            return new Node(data);
        }

        public string AsString()
        {
            return "[Node-> Name: " + data.objectPrefab.name + ", index: " + key + "]"; 
        }
    }
}


