using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableDataSO", menuName = "Purrcifer/Collectable SO/ConsumableSO")]
public class ConsumableDataSO : ScriptableObject
{
    [Header("Item Identifiers")]
    public string itemName = "Consumable item here.";

    [Header("Item Prefabs.")]
    public GameObject powerupPrefab;

    public int additiveHealthValue; 
}