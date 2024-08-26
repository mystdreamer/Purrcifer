using UnityEngine;

[System.Serializable]
public struct TreeDataSO
{
    public string name;
    public int ID;
    public PoolType poolType;
    public ItemTreeSO poolSO;
}

[System.Serializable]
[CreateAssetMenu(fileName = "MultiItemTreeSO", menuName = "Purrcifer/Scriptable Objects/MultiItemTreeSO")]
public class ItemMultiTreeSO : ScriptableObject
{
    public TreeDataSO[] trees;
}
