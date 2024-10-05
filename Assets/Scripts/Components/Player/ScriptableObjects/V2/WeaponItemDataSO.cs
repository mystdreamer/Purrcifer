using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponItemDataSO", menuName = "Purrcifer/Collectable SO/New WeaponItemDataSO")]
public class WeaponItemDataSO : ItemDataSO
{
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

    public void Apply() => GameManager.Instance.PlayerState.ApplyPowerup(this);
}
