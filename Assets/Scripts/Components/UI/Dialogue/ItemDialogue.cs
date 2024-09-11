using UnityEngine;

/// <summary>
/// Class for serializing item display text. 
/// </summary>
[System.Serializable]
public class ItemDialogue
{
    /// <summary>
    /// The name of the item. 
    /// </summary>
    public string itemName;

    /// <summary>
    /// The flavour text to accompany the item. 
    /// </summary>
    [TextArea(3, 10)]
    public string[] itemFlavourText;

}
