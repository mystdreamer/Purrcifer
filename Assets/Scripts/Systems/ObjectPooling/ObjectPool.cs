using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    /// <summary>
    /// The prefab type held by the pool. 
    /// </summary>
    public GameObject prefabType;

    /// <summary>
    /// The pool of objects. 
    /// </summary>
    public List<GameObject> pool;

    /// <summary>
    /// CTOR.
    /// </summary>
    /// <param name="prefabType"> The type of GameObject prefab to contain within the pool. </param>
    public ObjectPool(GameObject prefabType)
    {
        this.prefabType = prefabType;
        this.pool = new List<GameObject>();
    }

    /// <summary>
    /// Returns whether the pool is the same type. 
    /// </summary>
    /// <param name="prefabType"> The GameObject to locate. </param>
    /// <returns> True if pool is the same type. </returns>
    public bool PoolContainsType(GameObject prefabType) =>
        (prefabType == this.prefabType);

    /// <summary>
    /// Request an instance of the pools object type. 
    /// </summary>
    /// <returns> Instance if a valible, or null if not. </returns>
    public GameObject RequestInstance()
    {
        //Attempt to retrive an object from the object pool. 
        GameObject retrieved = AttemptPoolFetch();

        //If no object could be retrieved, then attempt to generate a new object. 
        if (retrieved == null)
        {
            retrieved = RequestNewObject();
            //Add instance to the pool. 
            pool.Add(retrieved);
        }

        //Return the instance. 
        return retrieved;
    }

    /// <summary>
    /// Search the pool to see if an object is active. 
    /// </summary>
    /// <returns> An instance of the object if the pool or null if not. </returns>
    private GameObject AttemptPoolFetch()
    {
        //Attempt to locate instance. 
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
                return pool[i];
        }

        //Else return null. 
        return null;
    }

    /// <summary>
    /// Generates a new instance of the GameObject. 
    /// </summary>
    /// <returns> New instance of the GameObject. </returns>
    private GameObject RequestNewObject() =>
        GameObject.Instantiate(prefabType);

    /// <summary>
    /// Clear the pool of all objects. 
    /// </summary>
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
