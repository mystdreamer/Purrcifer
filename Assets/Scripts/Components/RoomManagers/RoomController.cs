using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum representation of the rooms state. 
/// </summary>
public enum RoomState
{
    IDLE_ROOM,
    INACTIVE, 
    ACTIVE, 
    COMPLETED
}

/// <summary>
/// Required for interfacing with room objects. 
/// </summary>
public interface IRoomInterface
{
    /// <summary>
    /// Called when a room is awoken.  
    /// </summary>
    public void Awake();

    /// <summary>
    /// Used to check if the items purpose is complete. 
    /// </summary>
    /// <returns> True if completed. </returns>
    public bool IsComplete();

    /// <summary>
    /// Used to set the room objects to sleep. 
    /// </summary>
    public void Sleep();

    /// <summary>
    /// Used to notify if the world state has changed. 
    /// </summary>
    public void WorldStateChange();
}

public class RoomController : MonoBehaviour
{
    [System.Serializable]
    public struct RoomDetector
    {
        public int width;
        public int height;
        public Transform roomTransform;
        public Vector3 Center => roomTransform.position;

        public bool PlayerInRoom(Vector3 playerPos)
        {
            return (playerPos.x > Center.x - width / 2 &&
                playerPos.z > Center.z - height / 2 &&
                playerPos.x < Center.x + width / 2 &&
                playerPos.z < Center.z + height / 2);
        }
    }

    public RoomState roomState;
    private GameObject playerReference;
    public RoomDetector roomDetector;
    public IRoomInterface[] roomInterfaceObjects;
    public GameObject[] roomInterfaceItems;
    
    private Vector3 PlayerPosition
    {
        get
        {
            //If the reference isn't set update it. 
            if (playerReference == null)
            {
                playerReference = GameManager.Instance.playerCurrent;
            }
            return playerReference.transform.position;
        }
    }
    
    void Start()
    {   
        //Get a list of interfaces from the parent object. 
        List<IRoomInterface> interfaces = new List<IRoomInterface>();

        foreach (GameObject room in roomInterfaceItems)
        {
            interfaces.Add(gameObject.GetComponent<IRoomInterface>());
        }

        //Cache the references. 
        roomInterfaceObjects = interfaces.ToArray();
    }

    private void Update()
    {
        //State machine used for controlling room state. 
        if (roomState == RoomState.IDLE_ROOM)
            return;

        if (roomState == RoomState.INACTIVE)
            UpdateSleepState();
        
        if(roomState == RoomState.ACTIVE)
            UpdateActiveState();
    }

    /// <summary>
    /// Set the room back to its sleep state. 
    /// </summary>
    private void UpdateSleepState()
    {
        roomState = RoomState.ACTIVE;

        if (playerReference != null)
        {
            //Handle entrance check.
            if (roomDetector.PlayerInRoom(PlayerPosition))
            {
                //Room should be activated. 
                for (int i = 0; i < roomInterfaceObjects.Length; i++)
                {
                    roomInterfaceObjects[i].Awake();
                }
            }
        }
    }

    /// <summary>
    /// Function required for checking the rooms current completion state. 
    /// </summary>
    private void UpdateActiveState()
    {
        bool complete = true;

        for (int i = 0; i < roomInterfaceObjects.Length; i++)
        {
            if ( (complete = roomInterfaceObjects[i].IsComplete()) == false)
                break;
        }

        if (complete)
        {
            for(int i = 0;i < roomInterfaceObjects.Length; i++)
            {
                roomInterfaceObjects[i].Sleep();
            }
        }

        roomState = RoomState.COMPLETED;
    }
}
