using UnityEngine;
using ItemPool;

public class ItemPooling : MonoBehaviour
{
    public ProbPoolSO pool;
    public ItemBBT poolTree;
    public Range poolRange;

    private void Awake()
    {
        this.poolTree = new ItemBBT(pool);
        poolRange = new Range() { min = pool.probabilityRange.min, max = pool.probabilityRange.max };
    }
}
