namespace ItemPool
{
    public class Node<T>
    {
        public static readonly Node<T> NULL_NODE = null;
        public T item;
        public int key;
        public int weight;

        public Node<T> Left = NULL_NODE;
        public Node<T> Right = NULL_NODE;

        /// <summary>
        /// Returns the BBSTNode as a formatted string.  
        /// </summary>
        public string AsString() => "[Node-> Name: " + item.ToString() + ", index: " + key + "]";

        public Node(T data, int key, int weight)
        {
            item = data;
            this.key = key;
            this.weight = weight;
        }
    }
}