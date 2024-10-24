using UnityEngine;

public class CollisionStopper : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Stop movement
            rb.linearVelocity = Vector3.zero;
            // Stop rotation (if for some reason it isn't locked)
            rb.angularVelocity = Vector3.zero;
        }
    }
}
