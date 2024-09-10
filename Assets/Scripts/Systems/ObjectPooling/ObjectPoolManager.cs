using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject prefabType;
    public List<GameObject> pool;
    public int limitCap;

    public ObjectPool(GameObject prefabType)
    {
        this.prefabType = prefabType;
        this.pool = new List<GameObject>();
    }

    public bool PoolContainsType(GameObject prefabType) =>
        (prefabType == this.prefabType);

    public GameObject RequestInstance()
    {
        GameObject retrieved = AttemptPoolFetch();

        if (retrieved == null)
        {
            retrieved = RequestNewObject();
            pool.Add(retrieved);
        }

        return retrieved;
    }

    private GameObject AttemptPoolFetch()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
                return pool[i];
        }

        return null;
    }

    private GameObject RequestNewObject() =>
        GameObject.Instantiate(prefabType);

    public void EmptyPool()
    {
        GameObject obj;
        for (int i = pool.Count; i > 0; i--)
        {
            obj = pool[i];
            pool[i] = null;
            GameObject.Destroy(obj);
        }

        pool = new List<GameObject>();
    }
}

public class ObjectPoolManager : MonoBehaviour
{
    public List<ObjectPool> objectPools = new List<ObjectPool>();

    public GameObject GetGameObjectType(GameObject prefab)
    {
        ObjectPool lPool = LocatePool(prefab);
        GameObject obj = lPool.RequestInstance();
        return obj;
    }

    public GameObject GetGameObjectType(GameObject prefab, Vector2 position)
    {
        GameObject obj = GetGameObjectType(prefab);
        obj.transform.position = position;
        return obj;
    }

    public GameObject GetGameObjectType(GameObject prefab, GameObject parent, Vector2 position)
    {

        GameObject obj = GetGameObjectType(prefab, position);
        obj.transform.parent = parent.transform;
        return obj;
    }

    private ObjectPool LocatePool(GameObject prefab)
    {
        ObjectPool matchedPool = null;

        for (int i = 0; i < objectPools.Count; i++)
        {
            if (objectPools[i].PoolContainsType(prefab))
            {
                matchedPool = objectPools[i];
                break;
            }
        }

        if (matchedPool == null)
            matchedPool = AddPool(prefab);

        return matchedPool;
    }

    private ObjectPool AddPool(GameObject prefab)
    {
        ObjectPool p = new ObjectPool(prefab);
        objectPools.Add(p);
        return p;
    }

    public void ClearPools()
    {
        objectPools = new List<ObjectPool>();
    }
}
