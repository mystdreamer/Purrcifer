using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponDataSO", menuName = "Purrcifer/Collectable SO/WeaponDataSO")]
public class WeaponDataSO : ItemDataSO
{
    //Note any added item types to both WeaponType and UtilityType need to be added to 
    //Their respective functions in PlayerState ApplyPowerup(type).
    //Likewise any added value types need to be addressed in the PlayerState ApplyValueTypes(type)

    public enum WeaponType
    {
        SWORD = 0,
    }

    public WeaponType type;
    public bool singularPrefab = true;
    public GameObject weaponPrefab;

    public bool directionalPrefab = false;
    public GameObject weaponPrefabUp;
    public GameObject weaponPrefabDown;
    public GameObject weaponPrefabLeft;
    public GameObject weaponPrefabRight;
    public float destructionTime;
    public float cooldownTime;

    public void Apply() => GameManager.Instance.ApplyWeaponUpgrade = this;
}
