using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Purrcifer/Collectable SO/ItemDataSO")]
public class UtilityDataSO : ScriptableObject
{
    [Header("Item Identifiers")]
    public string itemName = "New util item name here";
    public int itemID = RandomIdGenerator.GetBase62(5);
    public PlayerEventData eventData;
    public ItemDialogue itemDialogue;

    [Header("Item Prefabs.")]
    public GameObject powerupPrefab;
    public GameObject utilityPrefab; 

    [Header("Modifiable powerup utility data.")]
    public int healthCap;
    public float playerSpeed;
    public int playerCharge;
    public float damageBase;
    public float damageMultiplier;
    public float damageCriticalHit;
    public float damageCriticalChance;

    [Header("Modifiable powerup utility data.")]
    public bool refillHealth;
}
