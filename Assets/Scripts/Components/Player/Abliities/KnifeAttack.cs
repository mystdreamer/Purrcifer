using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class KnifeAttack : TimedWeapons
{
    public Vector3 direction = Vector3.zero;
    public RaycastHit[] hits;
    public Weapon_4DirectionalPrefabs prefabs;

    internal override void Attack(Vector3 direction)
    {
        GameObject _rPrefab = prefabs.ResolvePrefab(direction);

        if (_rPrefab == null | !_canFire) return;
        _canFire = false;

        //Play attack sound. 
        if (SoundManager.Instance != null)
            SoundManager.Instance.OnAttack();

        GameObject _inst = GameObject.Instantiate(_rPrefab);
        _inst.transform.position = transform.position;
        _inst.transform.parent = transform;

        StartCoroutine(WeaponDisposer(_inst, prefabs.destructionTime));
        StartCoroutine(CoolDown(prefabs.cooldownTime));
    }
}