using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MultiItemTreeSO", menuName = "Purrcifer/Scriptable Objects/MultiItemTreeSO")]
public class ItemMultiTreeSO : ScriptableObject
{
    /// <summary>
    /// The tree data held within the scriptable object. 
    /// </summary>
    public ScriptableObj_MultiTreeData[] trees;
}
