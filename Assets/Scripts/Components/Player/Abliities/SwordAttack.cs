using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    STOPWATCH = 0,
    SWORD = 1,
}

[System.Serializable]
public struct Weapon_4DirectionalPrefabs
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public float destructionTime;
    public float cooldownTime;
}

public class SwordAttack : MonoBehaviour
{
    private const int RADIUS = 15;
    private const float LENGTH = 2;
    private const int NUMBER = 2;
    private const float OFFSET = 0.55F;
    private ItemType _type = ItemType.SWORD;
    private bool canFire = true;
    public Vector3 Direction = Vector3.zero;
    public Vector2[] directions;
    public RaycastHit[] hits;
    public Weapon_4DirectionalPrefabs prefabs;

    public ItemType Type => _type;

    public void Start()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(DefaultInputs.CTLR_Y) | Input.GetKeyDown(DefaultInputs.KEY_A_UP))
            Attack(prefabs.up, Vector3.up);

        if (Input.GetKeyDown(DefaultInputs.CTLR_A) | Input.GetKeyDown(DefaultInputs.KEY_A_DOWN))
            Attack(prefabs.down, Vector3.down);

        if (Input.GetKeyDown(DefaultInputs.CTLR_X) | Input.GetKeyDown(DefaultInputs.KEY_A_LEFT))
            Attack(prefabs.left, Vector3.left);

        if (Input.GetKeyDown(DefaultInputs.CTLR_B) | Input.GetKeyDown(DefaultInputs.KEY_A_RIGHT))
            Attack(prefabs.right, Vector3.right);
    }

    public void Attack(GameObject prefab, Vector3 vector)
    {
        if (vector == Vector3.zero | !canFire) return;

        GameObject inst = GameObject.Instantiate(prefab);
        inst.transform.position = transform.position;
        inst.transform.parent = transform;

        StartCoroutine(WeaponDisposer(inst, prefabs.destructionTime));
        StartCoroutine(CoolDown());

        Vector3[] directions = GetDirections(vector);
        RaycastHit[] hits;

        //Get hits for each vector direction. 
        for (int i = 0; i < directions.Length; i++)
        {
            hits = CastAndCheck(directions[i], OFFSET, LENGTH - OFFSET);
            
            //Check if hits are damageable interfaces. 
            for (int j = 0; j < hits.Length; j++)
            {
                Debug.Log("Hit GO: " +  hits[j].transform.gameObject);
                hits[j].collider.gameObject.GetComponent<IEntityInterface>()?.ApplyDamage(1);
            }
        }
    }

    private IEnumerator WeaponDisposer(GameObject prefab, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(prefab);
    }

    private IEnumerator CoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(prefabs.cooldownTime);
        canFire = true;
    }

    private Vector3[] GetDirections(Vector3 direction)
    {
        Vector3 initalRay = Quaternion.Euler(0, -(RADIUS / 2), 0) * direction;
        Quaternion rotQuat;
        List<Vector3> _directions = new List<Vector3>();
        float step = RADIUS / NUMBER;

        _directions.Add(initalRay);

        for (int i = 0; i <= NUMBER; i++)
        {
            rotQuat = Quaternion.Euler(0, step * i, 0);
            _directions.Add(rotQuat * initalRay);
        }
        return _directions.ToArray();
    }

    private RaycastHit[] CastAndCheck(Vector3 direction, float offset, float allowedDistance)
    {
        Ray ray = new Ray(transform.position + (direction.normalized * offset), direction.normalized);
        return Physics.RaycastAll(transform.position, direction, allowedDistance);
    }

    public void OnDrawGizmos()
    {
        Vector3 dir = Vector3.right;
        Vector3[] dirList = GetDirections(dir);

        List<RaycastHit> hits = new List<RaycastHit>();

        foreach (Vector3 direction in dirList)
        {
            Ray ray = new Ray(transform.position + (direction.normalized * OFFSET), direction.normalized * (LENGTH - OFFSET));
            Gizmos.DrawRay(ray);
            hits.AddRange(Physics.RaycastAll(ray));
        }

        foreach (RaycastHit hit in hits)
        {
            Gizmos.DrawSphere(hit.point, 0.025F);
        }
    }
}
