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

public class SqueakyHammer : TimedWeapons
{
    public HammerRadius hammerRadius;
    public GameObject hammerPrefab;
    public float weaponCooldown;
    public float weaponLifeTime;

    internal override void Attack(Vector3 direction)
    {
        Vector3 hammerPoint = transform.position + (direction.normalized * hammerRadius.radius);
        RaycastHit[] hits = Physics.SphereCastAll(hammerPoint, hammerRadius.radius, Vector3.up);

        //Create and position the hammer. 
        GameObject hammerInstance = Instantiate(hammerPrefab);
        Vector3 hammerPosition = gameObject.transform.position + (direction.normalized * hammerRadius.radius);

        //Calculate the player damage. 
        float damage = GameManager.Instance.PlayerState.Damage;

        //Detect hammer collision hits. 
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                GameObject obj = hits[i].collider.gameObject;

                //If is hit apply effects. 
                if (hammerRadius.InRadius(hammerPoint, obj.transform.position, out Vector3 _force))
                    ApplyHammerEffects(obj, damage, _force);
            }
        }

        StartCoroutine(CoolDown(weaponCooldown));
        StartCoroutine(WeaponDisposer(hammerInstance, weaponLifeTime));
    }

    private void ApplyHammerEffects(GameObject hitObj, float damage, Vector3 force)
    {
        hitObj.GetComponent<Rigidbody>().AddForce(force);
        Enemy e;
        if ((e = hitObj.GetComponent<Enemy>()) != null)
            e.CurrentHealth -= GameManager.Instance.PlayerState.Damage;

        Boss b;
        if ((b = hitObj.GetComponent<Boss>()) != null)
            b.BHealth.Health -= GameManager.Instance.PlayerState.Damage;
    }
}