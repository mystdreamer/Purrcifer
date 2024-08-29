using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MultiItemTreeSO", menuName = "Purrcifer/Scriptable Objects/MultiItemTreeSO")]
public class ItemMultiTreeSO : ScriptableObject
{
    public ScriptableObj_MultiTreeData[] trees;
}
