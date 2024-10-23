using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HammerRadius
{
    public float radius;

    public bool InRadius(Vector3 pointA, Vector3 pointB, out Vector3 force)
    {
        Vector3 directionVector = pointB - pointA;
        float distance = directionVector.magnitude;

        if (distance <= radius)
        {
            directionVector.Normalize();
            float _force = radius * (distance);
            force = directionVector * _force;
            return true;
        }
        force = Vector3.zero;
        return false;
    }
}

public class HammerHitCollider : MonoBehaviour
{
    float currentTime = 0;
    float sizeStep = 4 / 100;
    float damage;

    private void FixedUpdate()
    {
        transform.localScale = transform.localScale + (Vector3.one * sizeStep * Time.deltaTime);
    }

    public static GameObject GenerateHitSphere(HammerRadius hRadius, float damage)
    {
        GameObject effectMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        effectMarker.layer = LayerMask.NameToLayer("Weapon");
        SphereCollider coll = effectMarker.AddComponent<SphereCollider>();
        coll.radius = hRadius.radius;
        coll.isTrigger = true;
        HammerHitCollider hhc = effectMarker.AddComponent<HammerHitCollider>();
        hhc.damage = damage;
        return effectMarker;
    }
}

public class SqueakyHammer : TimedWeapons
{
    public HammerRadius hammerRadius;
    public Weapon_4DirectionalPrefabs prefabs;
    public float weaponCooldown;
    public float weaponLifeTime;

    internal override void Attack(Vector3 direction)
    {
        Vector3 hammerPoint = CalculateAttackPosition(direction);

        //Create and position the hammer. 
        GameObject hammerInstance = Instantiate(prefabs.ResolvePrefab(direction));
        GameObject visualization = HammerHitCollider.GenerateHitSphere(hammerRadius, GameManager.Instance.PlayerState.Damage);

        hammerInstance.transform.position = hammerPoint;
        visualization.transform.position = hammerPoint;

        //Detect hammer collision hits. 

        StartCoroutine(CoolDown(weaponCooldown));
        StartCoroutine(WeaponDisposer(hammerInstance, weaponLifeTime));
        StartCoroutine(WeaponDisposer(visualization, weaponLifeTime));
    }

    private Vector3 CalculateAttackPosition(Vector3 direction)
    {
        return transform.position + (direction.normalized * hammerRadius.radius);
    }

    private void OnDrawGizmos()
    {
        Vector3 upAttack = CalculateAttackPosition(Vector3.forward);
        Vector3 downAttack = CalculateAttackPosition(Vector3.back);
        Vector3 leftAttack = CalculateAttackPosition(Vector3.left);
        Vector3 rightAttack = CalculateAttackPosition(Vector3.right);

        Gizmos.DrawSphere(upAttack, hammerRadius.radius);
        Gizmos.DrawSphere(downAttack, hammerRadius.radius);
        Gizmos.DrawSphere(leftAttack, hammerRadius.radius);
        Gizmos.DrawSphere(rightAttack, hammerRadius.radius);
    }
}