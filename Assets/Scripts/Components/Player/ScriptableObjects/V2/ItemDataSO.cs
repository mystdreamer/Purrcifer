using JetBrains.Annotations;
using UnityEngine;

public abstract class ItemDataSO : ScriptableObject
{
    [Header("Item Identifiers")]
    public string itemName = "New name here";
    public int itemID = RandomIdGenerator.GetBase62(5);
    [Range(0, 100)]
    public int itemWeight; 
    public PlayerEventData eventData;
    public ItemDialogue itemDialogue;

    [Header("Item Prefabs.")]
    public GameObject powerupPedistoolPrefab;

    [Header("Stat change data.")]
    public StatChangeInt[] statChangeInts;
    public StatChangeFloat[] statChangeFloats;
    public StatChangeEffect[] statChangeEffects;
}
