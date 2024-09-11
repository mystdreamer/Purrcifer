using System.Collections;
using UnityEngine;

public class TeleportToGivenRoom : MonoBehaviour
{
    public MapIntMarkers markerToTeleportTo = MapIntMarkers.ROOM;
    public bool deactivateOnInteract;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            GameManager.Instance.Teleport(other.gameObject, markerToTeleportTo);
    }
}