using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Destroy(gameObject);
        }

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
