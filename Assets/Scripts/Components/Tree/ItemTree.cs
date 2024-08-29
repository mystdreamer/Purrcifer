using UnityEngine;
using ItemPool;
using JetBrains.Annotations;

public class ItemTree : MonoBehaviour
{
    public ItemTreeSO pool;
    public ItemBBST poolTree;
    public Range poolRange;

    private void Awake()
    {
        this.poolTree = new ItemBBST(pool);
        poolRange = new Range() { min = pool.probabilityRange.min, max = pool.probabilityRange.max };
    }

    public GameObject SpawnItem(Vector2 spawnPos, bool delete)
    {
        GameObject prefab = poolTree.GetRandomPrefab(delete);
        GameObject spawn = GameObject.Instantiate(prefab);
        spawn.transform.position = spawnPos;
        return spawn;
    }
}
