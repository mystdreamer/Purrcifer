using UnityEngine;
/// <summary>
/// The basic pool item data class. 
/// </summary>
[System.Serializable]
public class PoolItemData
{
    /// <summary>
    /// The items internal value. 
    /// </summary>
    public int key;
    
    /// <summary>
    /// The probability assigned to the item. 
    /// </summary>
    public int probability;

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
    public PoolItemData(int id, int probability, GameObject objectPrefab)
    {
        this.key = id;
        this.probability = probability;
        this.objectPrefab = objectPrefab;
    }
}
