using Purrcifer.BossAI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HammerRadius
{
    public float radius;
    public float force;
}

public class HammerHitCollider : MonoBehaviour
{
    HammerRadius hRadius;
    float currentTime = 0;
    float sizeStep = 4 / 100;
    float damage;

    private void FixedUpdate()
    {
        transform.localScale = transform.localScale + (Vector3.one * sizeStep * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision) => ResolveCollision(collision.gameObject);
    private void OnCollisionStay(Collision collision) => ResolveCollision(collision.gameObject);

    private void OnTriggerEnter(Collider other) => ResolveCollision(other.gameObject);
    private void OnTriggerStay(Collider other) => ResolveCollision(other.gameObject);

    private void ResolveCollision(GameObject other)
    {
        Debug.Log("Hit Collisions occured with " +  other.name);
        Vector3 directionVector = other.transform.position - gameObject.transform.position;
        Rigidbody body;
        other.TryGetComponent<Rigidbody>(out body);
        
        //if (body != null)
        //{
        //    StartCoroutine(ApplyForce(body, directionVector.normalized, hRadius.force));
        //}

        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            List<Enemy> enemyScripts = other.GetComponents<Enemy>().ToList();
            enemyScripts.AddRange(other.GetComponentsInChildren<Enemy>());

            foreach (Enemy enemy in enemyScripts)
            {
                enemy.CurrentHealth -= damage + GameManager.Instance.PlayerState.Damage;
            }

            List<BossHitbox> hitboxes = Helper_BossAI.GetHitBoxes(other);

            foreach (BossHitbox hitbox in hitboxes)
            {
                Debug.Log("Boss hitbox detected: " + hitbox.name);
                hitbox.ApplyDamage(damage + GameManager.Instance.PlayerState.Damage);
            }
        }
    }

    //private IEnumerator ApplyForce(Rigidbody body, Vector3 direction, float force)
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        if (body != null)
    //        {
    //            body.linearVelocity = Vector3.zero;
    //            body.AddForce(direction * force/10);
    //            yield return new WaitForFixedUpdate();
    //        }
    //        else
    //        {
    //            yield return true;
    //        }
    //    }
    //}

    public static GameObject GenerateHitSphere(HammerRadius hRadius, float damage)
    {
        GameObject effectMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        effectMarker.AddComponent<Rigidbody>();
        effectMarker.layer = LayerMask.NameToLayer("Weapon");
        SphereCollider coll = effectMarker.AddComponent<SphereCollider>();
        coll.radius = hRadius.radius;
        coll.isTrigger = true;
        HammerHitCollider hhc = effectMarker.AddComponent<HammerHitCollider>();
        hhc.damage = damage;
        hhc.hRadius = hRadius;
        return effectMarker;
    }
}

public class SqueakyHammer : TimedWeapons
{
    public HammerRadius hammerRadius;
    public Weapon_4DirectionalPrefabs prefabs;
    public float weaponCooldown;
    public float weaponLifeTime;
    public float damage;
    public AudioClip squeakySound;
    public AudioSource aSource; 

    internal override void Attack(Vector3 direction)
    {
        base._canFire = false;
        Vector3 hammerPoint = CalculateAttackPosition(direction);

        //Create and position the hammer. 
        GameObject hammerInstance = Instantiate(prefabs.ResolvePrefab(direction));
        GameObject visualization = HammerHitCollider.GenerateHitSphere(hammerRadius, GameManager.Instance.PlayerState.Damage);

        hammerInstance.transform.position = hammerPoint;
        visualization.transform.position = hammerPoint;
        aSource.PlayOneShot(squeakySound);
        //Detect hammer collision hits. 

        StartCoroutine(CoolDown(weaponCooldown));
        StartCoroutine(WeaponDisposer(hammerInstance, weaponLifeTime));
        StartCoroutine(WeaponDisposer(visualization, 0.8F));
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