using Purrcifer.Data.Defaults;
using System.Numerics;
using UnityEngine;

/// <summary>
/// Simple button for the pressing. 
/// </summary>
public class RoomButton : RoomObjectBase
{
    public override void OnAwakeObject() { }

    public override void OnSleepObject() { }

    internal override void SetWorldState(WorldStateEnum state) { }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Interactable)
        {
            Debug.Log("Item was interactable and collided with.");
            Complete = true;
        }
    }
}
