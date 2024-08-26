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
    public string itemName;

    /// <summary>
    /// The items internal value. 
    /// </summary>
    public int key;
    
    /// <summary>
    /// The probability assigned to the item. 
    /// </summary>
    public int probabilityWeight;

    /// <summary>
    /// The prefab to generate. 
    /// </summary>
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
