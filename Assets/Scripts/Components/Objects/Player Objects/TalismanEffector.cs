using System.Collections;
using UnityEngine;

public class TalismanEffector : MonoBehaviour
{
    public float maxRaidus;
    public float expansionTime;
    public SphereCollider radiusCollider;

    void Start()
    {
        StartCoroutine(ExpansionEffect());
    }

    public void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IOnTalisman>().Resolve();
    }

    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<IOnTalisman>().Resolve();
    }

    public IEnumerator ExpansionEffect()
    {
        float currentTime = 0;
        float currentRaidus = 0;

        while (currentRaidus < maxRaidus)
        {
            currentTime += Time.deltaTime;
            currentRaidus = maxRaidus * currentTime;
            radiusCollider.radius = currentRaidus;
            yield return new WaitForEndOfFrame();
        }

        radiusCollider.radius = maxRaidus; 
        yield return new WaitForSeconds(0.2F);

        gameObject.SetActive(false);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position, radiusCollider.radius);
    }
}
