using Purrcifer.Data.Defaults;
using Purrcifer.Data.Player;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public enum ItemType
{
    STOPWATCH = 0,
    SWORD = 1,
}

[System.Serializable]
public struct Weapon_4DirectionalPrefabs
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public float destructionTime;
    public float cooldownTime;
}

public class SwordAttack : TimedWeapons
{
    private const int RADIUS = 15;
    private const float LENGTH = 2;
    private const int NUMBER = 2;
    private const float OFFSET = 0.55F;
    private ItemType _type = ItemType.SWORD;
    public Vector3 direction = Vector3.zero;
    public RaycastHit[] hits;
    public Weapon_4DirectionalPrefabs prefabs;
    public ItemType Type => _type;

    private GameObject ResolvePrefab(Vector3 direction)
    {
        if(direction == Vector3.right) return prefabs.right;
        if(direction == Vector3.left) return prefabs.left;
        if(direction == Vector3.up) return prefabs.up;
        if(direction == Vector3.down) return prefabs.down;
        else return null;
    }

    internal override void Attack(Vector3 direction)
    {
        GameObject _rPrefab = ResolvePrefab(direction);

        if (_rPrefab == null | !_canFire) return;

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
