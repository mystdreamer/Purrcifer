using Purrcifer.Data.Defaults;
using System.Numerics;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour
{
    [SerializeField] private bool interactable = false;
    [SerializeField] private bool completed = false;
    public bool Complete
    {
        get => completed;
        internal set => completed = value;
    }
    public bool Interactable { get => interactable; }

    private void OnCollisionEnter(Collision collision)
    {
        if (!interactable) return;
        Event_Collision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.playerPrefab && interactable)
            Event_Triggered(other.gameObject);
    }

    public virtual void AwakenRoom()
    {
        interactable = true;
    }

    public virtual void SleepRoom()
    {
        interactable = false;
    }

    internal abstract void Event_Collision(GameObject collision);
    internal abstract void Event_Triggered(GameObject collision);

    internal abstract void SetWorldState(WorldStateEnum state);

    public string GetName()
    {
        return gameObject.name;
    }
}

/// <summary>
/// Simple button for the pressing. 
/// </summary>
public class RoomButton : RoomObject
{

    internal override void Event_Collision(GameObject collision)
    {
        if(collision.gameObject.tag == "Player" && Interactable)
        {
            Debug.Log("Item was interactable and collided with.");
            Complete = true;
        }
    }

    internal override void Event_Triggered(GameObject collision)
    {
    }

    internal override void SetWorldState(WorldStateEnum state)
    {

    }
}
