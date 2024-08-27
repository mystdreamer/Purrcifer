using JetBrains.Annotations;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{

    public FloorData floorData;
    public FloorPlan plan = null;

    void Start()
    {
        plan = MapBuilder.GenerateMap(floorData);
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            for (int x = 0; x < plan.plan.GetLength(0); x++)
            {
                for (int y = 0; y < plan.plan.GetLength(1); y++)
                {
                    if (plan.plan[x, y] == 1)
                        Gizmos.DrawSphere(new Vector3Int(x, y), 0.5f);
                }
            }
        }
    }
}
