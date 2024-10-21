using Unity.VisualScripting;
using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    public Boss boss;

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnCollisionStay(Collision collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        
    }

    public void ResolveCollisions(GameObject collisionObject)
    {
        if(collisionObject.tag == "Weapon")
        {
            boss.CurrentHealth -= GameManager.Instance.PlayerState.Damage;
        }
    }
}
