using ItemPool;
using UnityEngine;

/// <summary>
/// Internal class for handling tree data structures. 
/// </summary>
[System.Serializable]
public class TreeData
{
    [SerializeField] private string _name;
    [SerializeField] private int _ID;
    [SerializeField] private ItemTreeType _poolType;
    [SerializeField] private ItemBBST _itemTree;

    public string Name => _name;
    public int ID => _ID;
    public ItemTreeType PoolType => _poolType;
    public ItemBBST ItemTree => _itemTree;

    public TreeData(ScriptableObj_MultiTreeData data)
    {
        this._name = data.name;
        this._ID = data.ID;
        this._poolType = data.poolType;
        this._itemTree = new ItemBBST(data.poolSO);
    }

    public static explicit operator TreeData(ScriptableObj_MultiTreeData instanceData)
    {
        return new TreeData(instanceData);
    }
}
