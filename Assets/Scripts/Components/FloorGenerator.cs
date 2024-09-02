using JetBrains.Annotations;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public FloorData floorData;

    public void Start()
    {
        GameManager.FloorData = floorData;
    }

}
