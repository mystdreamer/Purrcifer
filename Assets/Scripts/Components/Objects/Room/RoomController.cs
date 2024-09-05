using System.Collections;
using UnityEngine;

/// <summary>
/// Enum representation of the rooms state. 
/// </summary>
public enum RoomState
{
    IDLE_ROOM,
    INACTIVE,
    ACTIVE,
    COMPLETED,
    TRANSITIONING
}

public class RoomController : MonoBehaviour
{
    [System.Serializable]
    public struct RoomDetector
    {
        const float WIDTH = 35f;
        const float HEIGHT = 19f;

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

    private const float ROOM_CLOSE_DELAY = 1.5f;
    private const float ROOM_OPEN_DELAY = 0.15f;
    public RoomState roomState;
    private GameObject playerReference;
    public RoomDetector roomDetector;
    public RoomObject[] roomObjects;

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

    private void Update()
    {
        if (Player == null | roomState == RoomState.COMPLETED)
            return;

        StateMachine();
    }

    /// <summary>
    /// State machine used for controlling the rooms state. 
    /// </summary>
    private void StateMachine()
    {
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
            if (ItemsCompleted())
                return;

            Debug.Log("Player Detected In room.");
            roomState = RoomState.TRANSITIONING;
            StartCoroutine(EnableDelayOperation(ROOM_OPEN_DELAY));
        }
    }

    /// <summary>
    /// Function required for checking the rooms current completion state. 
    /// </summary>
    private void UpdateActiveState()
    {
        bool roomContentsCheck = ItemsCompleted();
        if (roomContentsCheck)
        {
            roomState = RoomState.TRANSITIONING;
            StartCoroutine(DisableDelayOperation(ROOM_CLOSE_DELAY));
        }
    }

    private IEnumerator EnableDelayOperation(float time)
    {
        Debug.Log("Beginning Delay On Room Enter.");
        yield return new WaitForSeconds(time);
        EnableInterfaces();
    }

    private IEnumerator DisableDelayOperation(float time)
    {
        Debug.Log("Beginning Delay On Room Exit.");
        yield return new WaitForSeconds(time);
        DisableInterfaces();
    }

    private void EnableInterfaces()
    {
        Debug.Log("Activating objects.");
        //Room should be activated. 
        SetRoomContentsEnableState(true);
        roomState = RoomState.ACTIVE;
    }

    private void DisableInterfaces()
    {
        Debug.Log("Deactivating objects.");
        //Room should be deactivated. 
        SetRoomContentsEnableState(false);
        roomState = RoomState.COMPLETED;
    }

    private bool ItemsCompleted()
    {
        for (int i = 0; i < roomObjects.Length; i++)
        {
            //Debug.Log("Room State Object Check: " + roomObjects[i].GetName() + " - " + roomObjects[i].Complete);
            if (!roomObjects[i].Complete)
                return false;
        }

        return true;
    }

    private void SetRoomContentsEnableState(bool state)
    {
        for (int i = 0; i < roomObjects.Length; i++)
        {
            if (state == true)
                roomObjects[i].AwakenRoom();
            else if (state == false)
                roomObjects[i].SleepRoom();
        }
    }

    private void OnDrawGizmos()
    {
        roomDetector.Draw();
    }
}
