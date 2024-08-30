using UnityEngine;

/// <summary>
/// Simple button for the pressing. 
/// </summary>
public class RoomButton : MonoBehaviour, IRoomInterface
{
    public bool interactable = false;
    public bool complete = false; 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameManager.Instance.playerPrefab && interactable)
        {
            complete = true;    
        }
    }

    void IRoomInterface.Awake()
    {
        interactable = true;
    }

    bool IRoomInterface.IsComplete()
    {
        return complete;
    }

    void IRoomInterface.Sleep()
    {
        interactable = false; 
    }

    void IRoomInterface.WorldStateChange()
    {

    }
}
