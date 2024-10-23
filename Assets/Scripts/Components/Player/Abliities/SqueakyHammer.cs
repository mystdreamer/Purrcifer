
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


public class SqueakyHammer : MonoBehaviour
{
    public HammerRadius hammerRadius;
    public GameObject hammerPrefab;


    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ApplyStrike(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ApplyStrike(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ApplyStrike(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ApplyStrike(Vector3.back);
        }
    }

    private void ApplyStrike(Vector3 direction)
    {
        Vector3 hammerPoint = transform.position + (direction.normalized * hammerRadius.radius);
        RaycastHit[] hits = Physics.SphereCastAll(hammerPoint, hammerRadius.radius, Vector3.up);

        //Add object positioning. 

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                GameObject obj = hits[i].collider.gameObject;
                Vector3 _force;
                bool result = hammerRadius.InRadius(hammerPoint, obj.transform.position, out _force);
                if (result)
                {
                    obj.GetComponent<Rigidbody>().AddForce(_force);
                }
                Enemy e;
                if ((e = obj.GetComponent<Enemy>()) != null)
                {
                    e.CurrentHealth -= GameManager.Instance.PlayerState.Damage;
                }

                Boss b;
                if ((b = obj.GetComponent<Boss>()) != null)
                {
                    b.BHealth.Health -= GameManager.Instance.PlayerState.Damage;
                }
            }
        }
    }
}