using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Purrcifer/Collectable SO/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Item Identifiers")]
    public string itemName = "New weapon name here";
    public string weaponID = RandomIdGenerator.GetBase62(5);
    public PlayerEventData eventData;
    public ItemDialogue itemDialogue; 

    [Header("Item Prefabs.")]
    public GameObject powerupPrefab;
    
    public bool singularPrefab = true;
    public GameObject weaponPrefab;
    
    public bool directionalPrefab = false;
    public GameObject weaponPrefabUp;
    public GameObject weaponPrefabDown;
    public GameObject weaponPrefabLeft;
    public GameObject weaponPrefabRight;

    [Header("Modifiable powerup weapon data.")]
    public float damageBase;
    public float damageMultiplier;
    public float damageCriticalHit;
    public float damageCriticalChance;
}
