using System.Collections;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] private bool playerLeftTeleporter = true;
    [SerializeField] private bool pointActive = true;
    public bool deactivateOnArrival;
    public bool deactivateOnInteract;
    public TeleportPoint receiver;

    public Vector3 Position => gameObject.transform.position;

    public bool EnablePoint
    {
        set => pointActive = true;
    }

    private bool CanTeleport => playerLeftTeleporter && pointActive;

    public void TeleportHere(GameObject teleportObject)
    {
        teleportObject.transform.position = Position;

        pointActive = false;
        playerLeftTeleporter = false;

        if (!deactivateOnArrival)
        {
            StartCoroutine(TeleportCooldown());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!CanTeleport)
                return;

            //Teleport player. 
            receiver.TeleportHere(other.gameObject);
            pointActive = false;
            playerLeftTeleporter = false;

            //Resolve teleport conditions. 
            if (!deactivateOnInteract)
            {
                //If should reset, start cooldown timer. 
                StartCoroutine(TeleportCooldown());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerLeftTeleporter = true;
    }

    public void OnDrawGizmos()
    {
        if (receiver != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(Position, receiver.Position);
        }
    }

    public IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(0.05f);
        pointActive = true;
    }
}