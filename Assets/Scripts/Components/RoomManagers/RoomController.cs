using NUnit.Framework;
using System.Collections;
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
        const float WIDTH = 17.5F;
        const float HEIGHT = 9.5F;

        public Transform roomTransform;
        public Vector3 Center => roomTransform.position;
        public float MinX => Center.x - WIDTH / 2;
        public float MaxX => Center.x + WIDTH / 2;
        public float MinZ => Center.z - HEIGHT / 2;
        public float MaxZ => Center.z + HEIGHT / 2;

        public bool PlayerInRoom(Vector3 playerPos)
        {
            return (playerPos.x > MinX && playerPos.z > MinZ &&
                playerPos.x < MaxX &&
                playerPos.z < MaxZ);
        }

        public void Draw()
        {
            Gizmos.DrawWireCube(Center, new Vector3(WIDTH, 0, HEIGHT));
        }
    }

    public RoomState roomState;
    private GameObject playerReference;
    public RoomDetector roomDetector;
    public IRoomInterface[] roomInterfaceObjects;
    public GameObject[] roomInterfaceItems;

    private GameObject Player
    {
        get
        {
            if (playerReference == null)
            {
                playerReference = GameManager.Instance.playerCurrent;
            }

            return playerReference;
        }
    }

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
        if (Player == null)
        {
            Debug.Log("Player not found.");
            return;
        }
        else
        {
            StateMachine();
        }
    }

    /// <summary>
    /// State machine used for controlling the rooms state. 
    /// </summary>
    private void StateMachine()
    {
        if (roomState == RoomState.IDLE_ROOM)
            return;

        if (roomState == RoomState.INACTIVE)
            UpdateSleepState();

        if (roomState == RoomState.ACTIVE)
            UpdateActiveState();
    }

    /// <summary>
    /// Set the room back to its sleep state. 
    /// </summary>
    private void UpdateSleepState()
    {
        //Handle entrance check.
        if (roomDetector.PlayerInRoom(PlayerPosition))
        {
            roomState = RoomState.ACTIVE;
            StartCoroutine(EnableDelayOperation(2.5f));
            EnableInterfaces();
        }
    }

    /// <summary>
    /// Function required for checking the rooms current completion state. 
    /// </summary>
    private void UpdateActiveState()
    {
        foreach (IRoomInterface room in roomInterfaceObjects)
        {
            if (room.IsComplete() != true)
                return;
        }

        StartCoroutine(DisableDelayOperation(2.5f));
        roomState = RoomState.COMPLETED;
    }

    private IEnumerator EnableDelayOperation(float time)
    {
        yield return new WaitForSeconds(time);
        EnableInterfaces();
    }

    private IEnumerator DisableDelayOperation(float time)
    {
        yield return new WaitForSeconds(time);
        DisableInterfaces();
    }

    private void EnableInterfaces()
    {
        //Room should be activated. 
        for (int i = 0; i < roomInterfaceObjects.Length; i++)
        {
            roomInterfaceObjects[i].Awake();
        }
    }

    private void DisableInterfaces()
    {
        //Room should be activated. 
        for (int i = 0; i < roomInterfaceObjects.Length; i++)
        {
            roomInterfaceObjects[i].Sleep();
        }
    }

    private void OnDrawGizmos()
    {
        roomDetector.Draw();
    }
}
