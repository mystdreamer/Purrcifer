using Purrcifer.Data.Defaults;
using System.Numerics;
using UnityEngine;

/// <summary>
/// Simple button for the pressing. 
/// </summary>
public class RoomButton : RoomObjectBase
{
    internal override void OnAwakeObject() { }

    internal override void OnSleepObject() { }

    internal override void WorldUpdateReceiver(WorldState state)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && ObjectActive)
        {
            Debug.Log("Item was interactable and collided with.");
            ObjectComplete = true;
        }
    }
}
