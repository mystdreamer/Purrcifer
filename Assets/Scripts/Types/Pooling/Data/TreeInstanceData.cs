using ItemPool;
using UnityEngine;

[System.Serializable]
public partial class MultiTree
{
    /// <summary>
    /// Internal class for handling tree data structures. 
    /// </summary>
    public class TreeInstanceData
    {
        [SerializeField] private string _name;
        [SerializeField] private int _ID;
        [SerializeField] private PoolType _poolType;
        [SerializeField] private ItemBBST _itemTree;

        public string Name => _name;
        public int ID => _ID;   
        public PoolType PoolType => _poolType;  
        public ItemBBST ItemTree => _itemTree;

        public TreeInstanceData(TreeDataSO data)
        {
            this._name = data.name;
            this._ID = data.ID;
            this._poolType = data.poolType;
            this._itemTree = new ItemBBST(data.poolSO);
        }

        public static explicit operator TreeInstanceData(TreeDataSO instanceData)
        {
            return new TreeInstanceData(instanceData);
        }
    }

}
