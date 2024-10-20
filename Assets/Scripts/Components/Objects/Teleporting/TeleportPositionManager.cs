using System.Collections.Generic;
using UnityEngine;

public class TeleportPositionManager : MonoBehaviour
{
    public Transform[] teleportTransforms;

    public Vector3 GetRandomSpawn()
    {
        if (teleportTransforms.Length == 0)
        {
            if (teleportTransforms[0] == null)
                return Vector3.negativeInfinity;
        }
        else
        {
            return teleportTransforms[UnityEngine.Random.Range(0, teleportTransforms.Length)].position;
        }

        return Vector3.negativeInfinity;
    }
}
