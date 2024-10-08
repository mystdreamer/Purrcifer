using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object data structure for defining pool contents. 
/// </summary>
[CreateAssetMenu(order = 0, fileName = "ItemPoolSO", menuName = "Purrcifer/Scriptable Objects/ItemTreeSO")]
public class ItemTreeSO : ScriptableObject
{

    /// <summary>
    /// The list of items held within the pool. 
    /// </summary>
    public List<ItemDataSO> itemData;
}
