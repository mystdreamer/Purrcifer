using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect; // The impact effect to instantiate

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Instantiate the impact effect at the collision point
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.identity);

            // Destroy the current GameObject
            Destroy(gameObject);
        }
    }
}
