using Unity.VisualScripting;
using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    public Boss boss;

    public void OnCollisionEnter(Collision collision)
    {
        ResolveCollisions(collision.gameObject);
    }

    public void OnCollisionStay(Collision collision)
    {
        ResolveCollisions(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        ResolveCollisions(other.gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        ResolveCollisions(other.gameObject);
    }

    public void ResolveCollisions(GameObject collisionObject)
    {
        if (collisionObject.tag == "Weapon")
        {
            Debug.Log("Boss collision hit: Applying damage.");
            boss.Health -= GameManager.Instance.PlayerState.Damage;
        }
    }

    public void ApplyDamage(float value)
    {
        Debug.Log("Boss collision hit: Applying damage.");
        boss.Health -= value;
    }
}
