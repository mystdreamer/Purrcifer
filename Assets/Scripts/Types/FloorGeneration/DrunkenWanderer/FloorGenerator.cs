using JetBrains.Annotations;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{

    public FloorData floorData;
    public FloorPlan plan = null;

    void Start()
    {
        Generate();
        FloorPlan.Print2DArray<int>(plan.plan);
        FloorPlan.Print2DArray<int>(plan.GetEndpointMap());

        ApplyStart applyStartDec = new ApplyStart();
        DefineExit exitDec = new DefineExit();

        bool startSet;
        applyStartDec.Decorate(plan, out startSet);
        FloorPlan.Print2DArray<int>(plan.plan);

        bool bossSet;
        exitDec.Decorate(plan, out bossSet);
        Debug.Log("Adding boss room state: " + bossSet);
        FloorPlan.Print2DArray<int>(plan.plan);
    }

    void Update()
    {

    }

    void Generate()
    {
        plan = DrunkenWanderer.GenerateFloorMap(floorData);
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
