using UnityEngine;

public class ItemMultiTree : MonoBehaviour
{
    public MultiTree itemMultiTree;
    public ItemMultiTreeSO multiTree;

    private void Awake()
    {
        itemMultiTree = new MultiTree(multiTree);
    }

    public GameObject SpawnItem(Vector2 spawnPos, bool delete)
    {
        GameObject prefab = itemMultiTree.GetRandomPrefab(delete);
        GameObject spawn = GameObject.Instantiate(prefab);
        spawn.transform.position = spawnPos;
        return spawn;
    }
}
