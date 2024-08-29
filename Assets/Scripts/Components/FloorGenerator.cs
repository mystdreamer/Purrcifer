using JetBrains.Annotations;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{

    public FloorData floorData;
    public FloorPlan plan = null;

    public void Start()
    {
        GameManager.Instance.GenerateRandomMap(floorData);
    }

}
