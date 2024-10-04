using System.Collections;
using UnityEngine;

public class TalismanEffector : MonoBehaviour
{
    public float maxRaidus;
    public float expansionTime;
    public SphereCollider radiusCollider;

    void Start()
    {
        radiusCollider.radius = maxRaidus;
        StartCoroutine(ExpansionEffect());
    }

    public IEnumerator ExpansionEffect()
    {
        float currentTime = 0;
        float currentRaidus = maxRaidus;

        while (currentRaidus > 0)
        {
            currentTime += Time.deltaTime;
            currentRaidus -= maxRaidus * currentTime;
            radiusCollider.radius = currentRaidus;
            GetCastResults(transform.position, radiusCollider.radius);
            yield return new WaitForEndOfFrame();
        }

        radiusCollider.radius = 0; 
        yield return new WaitForSeconds(0.2F);

        gameObject.SetActive(false);
    }

    private void GetCastResults(Vector3 position, float radius)
    {
        RaycastHit[] results = Physics.SphereCastAll(position, radius, transform.forward);
        GameObject obj;

        foreach (RaycastHit hit in results)
        {
            obj = hit.transform.gameObject;
            Debug.Log("Talisman collision resolution called");
            if (obj.GetComponent<TalismanDestructable>() != null)
            {
                Debug.Log("Abstract class found");
                obj.GetComponent<TalismanDestructable>().Resolve();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position, radiusCollider.radius);
    }
}
