using UnityEngine;

/// <summary>
/// The basic pool item data class. 
/// </summary>
[System.Serializable]
public class TreeItemData
{
    /// <summary>
    /// The name of the item. 
    /// </summary>
    [Header("The name of the item.")]
    public string itemName;

    /// <summary>
    /// The items internal value. 
    /// </summary>
    [Header("The ID key of the item.")]
    public int key;
    
    /// <summary>
    /// The probability assigned to the item. 
    /// </summary>
    [Header("The weighted probability of the tree.")]
    public int probabilityWeight;

    /// <summary>
    /// The prefab to generate. 
    /// </summary>
    [Header("The object to spawn.")]
    public GameObject objectPrefab;

    /// <summary>
    /// CTOR:
    /// </summary>
    /// <param name="id"> The id of the object. </param>
    /// <param name="probability"> The probability weighting assigned to the object. </param>
    /// <param name="objectPrefab"> The prefab to generate. </param>
    public TreeItemData(int id, int probability, GameObject objectPrefab)
    {
        this.key = id;
        this.probabilityWeight = probability;
        this.objectPrefab = objectPrefab;
    }
}
