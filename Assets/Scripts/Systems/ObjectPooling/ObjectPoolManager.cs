using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolManager
{
    /// <summary>
    /// The list of objects held within the manager. 
    /// </summary>
    [SerializeField] private List<ObjectPool> objectPools = new List<ObjectPool>();

    /// <summary>
    /// Generate an object instance.
    /// </summary>
    /// <param name="prefab"> The prefab to generate. </param>
    /// <returns> An instance of the requested GameObject. </returns>
    public GameObject GetGameObjectType(GameObject prefab)
    {
        //Retrieve the correct pool from the pool list. 
        ObjectPool lPool = LocatePool(prefab);

        //Request an instance from the pool. 
        return lPool.RequestInstance();
    }

    /// <summary>
    /// Generate an object instance.
    /// </summary>
    /// <param name="prefab"> The prefab to generate. </param>
    /// <param name="position"> The position to assign to the object. </param>
    /// <returns> An instance of the requested GameObject. </returns>
    public GameObject GetGameObjectType(GameObject prefab, Vector2 position)
    {
        //Get object instance. 
        GameObject obj = GetGameObjectType(prefab);

        //Set position. 
        obj.transform.position = position;
        return obj;
    }

    /// <summary>
    /// Generate an object instance.
    /// </summary>
    /// <param name="prefab"> The prefab to generate. </param>
    /// <param name="position"> The position to assign to the object. </param>
    /// <param name="parent"> The parent of the prefab. </param>
    /// <returns> An instance of the requested GameObject. </returns>
    public GameObject GetGameObjectType(GameObject prefab, Vector2 position, GameObject parent)
    {
        //Get object instance. 
        GameObject obj = GetGameObjectType(prefab, position);
        
        //Set parent. 
        obj.transform.parent = parent.transform;
        return obj;
    }

    /// <summary>
    /// Returns a pool with the requested GameObject type. 
    /// </summary>
    /// <param name="prefab"> The prefab to find the pool of. </param>
    private ObjectPool LocatePool(GameObject prefab)
    {
        //Cache null pool reference. 
        ObjectPool matchedPool = null;

        //Attempt to match type. 
        for (int i = 0; i < objectPools.Count; i++)
        {
            if (objectPools[i].PoolContainsType(prefab))
            {
                matchedPool = objectPools[i];
                break;
            }
        }

        //If the pool was not located, then generate a new pool instance. 
        if (matchedPool == null)
            matchedPool = AddPool(prefab);

        //Return the pool. 
        return matchedPool;
    }

    /// <summary>
    /// Add a ObjectPool to the manager. 
    /// </summary>
    /// <param name="prefab"> The prefab the pool contains. </param>
    /// <returns> A new ObjectPool. </returns>
    private ObjectPool AddPool(GameObject prefab)
    {
        //Generate the pool. 
        ObjectPool p = new ObjectPool(prefab);

        //Add it to the manager. 
        objectPools.Add(p);
        return p;
    }

    /// <summary>
    /// Clear the pool with the provided type. 
    /// </summary>
    /// <param name="prefab"> The type of GameObject to clear from the pool. </param>
    public void ClearPoolByType(GameObject prefab)
    {
        ObjectPool pool = LocatePool(prefab);
        pool.EmptyPool();
    }

    /// <summary>
    /// Clear all pools from the list. 
    /// </summary>
    public void ClearPools()
    {
        objectPools = new List<ObjectPool>();
    }
}
