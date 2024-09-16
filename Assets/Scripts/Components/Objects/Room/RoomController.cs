using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Purrcifer.Data.Defaults;
using Room.WallController;

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

#region Room Collision.
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
#endregion

public class RoomController : MonoBehaviour
{
    private const float ROOM_CLOSE_DELAY = 1.5f;
    private const float ROOM_OPEN_DELAY = 0.15f;
    public RoomState roomState;
    private GameObject playerReference;
    public RoomDetector roomDetector;
    public RoomObjectBase[] roomObjects;
    private NavMeshSurface navMeshSurface;

    #region Properties. 
    private GameObject Player
    {
        get
        {
            if (playerReference == null)
            {
                playerReference = GameManager.Instance.Player;
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
                playerReference = GameManager.Instance.Player;
            }
            return playerReference.transform.position;
        }
    }
    #endregion

    private void Awake()
    {
        // Get or add NavMeshSurface component
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        }

        // Configure NavMeshSurface
        navMeshSurface.collectObjects = CollectObjects.Children;
        navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;

        // Bake NavMesh for this room
        BakeNavMesh();
    }

    private void Update()
    {
        if (Player == null | roomState == RoomState.COMPLETED)
            return;

        StateMachine();
    }

    #region Door Control.

    [SerializeField] private WallType RoomMarkerType;

    /// <summary>
    /// The door heading upward. 
    /// </summary>
    public RoomWallData up;

    /// <summary>
    /// The door heading downwards. 
    /// </summary>
    public RoomWallData down;

    /// <summary>
    /// The door heading left. 
    /// </summary>
    public RoomWallData left;

    /// <summary>
    /// The door heading right. 
    /// </summary>
    public RoomWallData right;

    /// <summary>
    /// Lock/Unlock the room. 
    /// </summary>
    public bool SetLockState
    {
        set
        {
            up.SetDoorLockState = value;
            down.SetDoorLockState = value;
            right.SetDoorLockState = value;
            left.SetDoorLockState = value;
        }
    }

    public WallType MarkerType
    {
        get => RoomMarkerType;
        set
        {
            RoomMarkerType = value;
            right.WallType = value;
            left.WallType = value;
            up.WallType = value;
            down.WallType = value;
        }
    }

    /// <summary>
    /// Set the given side to be a door. 
    /// </summary>
    /// <param name="direction"> The side to set. </param>
    public void SetRoomState(WallDirection direction, WallType type)
    {
        switch (direction)
        {
            case WallDirection.LEFT:
                left.WallType = type;
                break;
            case WallDirection.RIGHT:
                right.WallType = type;
                break;
            case WallDirection.UP:
                up.WallType = type;
                break;
            case WallDirection.DOWN:
                down.WallType = type;
                break;
        }
    }

    #endregion

    #region Bake Navigation Meshes. 
    private void BakeNavMesh()
    {
        // Check if NavMeshSurface is available
        if (navMeshSurface != null)
        {
            try
            {
                // Attempt to build/bake the NavMesh
                navMeshSurface.BuildNavMesh();
                Debug.Log("NavMesh successfully baked for room: " + gameObject.name);
            }
            catch (System.Exception ex)
            {
                // Catch any exceptions and log the error
                Debug.LogError("NavMesh baking failed for room: " + gameObject.name + ". Exception: " + ex.Message);
            }
        }
        else
        {
            // Log an error if NavMeshSurface is missing
            Debug.LogError("NavMeshSurface is missing on room: " + gameObject.name + ". Baking aborted.");
        }
    }
    #endregion

    #region State Setting. 
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
        SetLockState = true;
        EnableInterfaces();
    }

    private IEnumerator DisableDelayOperation(float time)
    {
        Debug.Log("Beginning Delay On Room Exit.");
        yield return new WaitForSeconds(time);
        SetLockState = false;
        DisableInterfaces();
    }
    #endregion

    #region Object interface control. 

    private void EnableInterfaces()
    {
        //Debug.Log("Activating objects.");
        //Room should be activated. 
        SetRoomContentsEnableState(true);
        roomState = RoomState.ACTIVE;
    }

    private void DisableInterfaces()
    {
        //Debug.Log("Deactivating objects.");
        //Room should be deactivated. 
        SetRoomContentsEnableState(false);
        roomState = RoomState.COMPLETED;
    }

    private bool ItemsCompleted()
    {
        for (int i = 0; i < roomObjects.Length; i++)
        {
            //Debug.Log("Room State Object Check: " + roomObjects[i].GetName() + " - " + roomObjects[i].Complete);
            if (!roomObjects[i].ObjectComplete)
                return false;
        }

        return true;
    }

    private void SetRoomContentsEnableState(bool state)
    {
        for (int i = 0; i < roomObjects.Length; i++)
        {
            if (state == true)
                ((IRoomObject)roomObjects[i]).AwakenObject();
            else if (state == false)
                ((IRoomObject)roomObjects[i]).SleepObject();
        }
    }
    #endregion

    #region Gizmos drawing.
    private void OnDrawGizmos()
    {
        roomDetector.Draw();
    }
    #endregion
}
