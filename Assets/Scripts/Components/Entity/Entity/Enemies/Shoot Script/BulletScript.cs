using Purrcifer.Data.Defaults;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
