namespace ItemPool
{
    [System.Serializable]
    public class Node<T>
    {
        public T item;
        public int key;
        public int weight;

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