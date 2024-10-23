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
    private bool hasPlayedCompletionSound = false;

    private float activeStateCheckInterval = 5f;
    private float lastActiveStateCheckTime;

    #region Properties
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
        Debug.Log($"Room {gameObject.name}: Initialized with state {roomState}");
    }

    private void Update()
    {
        if (Player == null || roomState == RoomState.COMPLETED)
            return;

        StateMachine();

        // Check UpdateActiveState every 5 seconds when the room is active
        if (roomState == RoomState.ACTIVE && Time.time - lastActiveStateCheckTime >= activeStateCheckInterval)
        {
            UpdateActiveState();
            lastActiveStateCheckTime = Time.time;
        }
    }

    #region Door Control
    [SerializeField] private WallType wallType;
    public MapIntMarkers roomType;

    public RoomWallData up;
    public RoomWallData down;
    public RoomWallData left;
    public RoomWallData right;

    public bool SetLockState
    {
        set
        {
            Debug.Log($"Room {gameObject.name}: Setting lock state to {value}");
            up.SetDoorLockState = value;
            down.SetDoorLockState = value;
            right.SetDoorLockState = value;
            left.SetDoorLockState = value;
        }
    }

    public WallType MarkerType
    {
        get => wallType;
        set
        {
            wallType = value;
            right.WallType = value;
            left.WallType = value;
            up.WallType = value;
            down.WallType = value;
        }
    }

    public void SetRoomState(WallDirection direction, WallType type)
    {
        //Debug.Log($"Room {gameObject.name}: Setting wall {direction} to type {type}");
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

    #region Nav Mesh
    private void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            try
            {
                navMeshSurface.BuildNavMesh();
                Debug.Log($"Room {gameObject.name}: NavMesh baked successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Room {gameObject.name}: NavMesh baking failed - {ex.Message}");
            }
        }
    }
    #endregion

    #region State Management
    private void StateMachine()
    {
        if (roomState == RoomState.INACTIVE)
        {
            UpdateSleepState();
        }
        else if (roomState == RoomState.ACTIVE)
        {
            UpdateActiveState();
        }
    }

    private void UpdateSleepState()
    {
        if (roomDetector.PlayerInRoom(PlayerPosition))
        {
            if (ItemsCompleted())
            {
                //Debug.Log($"Room {gameObject.name}: Already completed, staying in sleep state");
                return;
            }

            //Debug.Log($"Room {gameObject.name}: Player entered, transitioning from sleep state");
            roomState = RoomState.TRANSITIONING;
            StartCoroutine(EnableDelayOperation(ROOM_OPEN_DELAY));
        }
    }

    private void UpdateActiveState()
    {
        bool roomContentsCheck = ItemsCompleted();
        if (roomContentsCheck && roomState != RoomState.TRANSITIONING)
        {
            //Debug.Log($"Room {gameObject.name}: Room completed, transitioning from active state");
            roomState = RoomState.TRANSITIONING;
            StartCoroutine(DisableDelayOperation(ROOM_CLOSE_DELAY));
        }
    }

    private IEnumerator EnableDelayOperation(float time)
    {
        //Debug.Log($"Room {gameObject.name}: Starting enable delay");
        yield return new WaitForSeconds(time);
        SetLockState = true;
        EnableInterfaces();
    }

    private IEnumerator DisableDelayOperation(float time)
    {
        //Debug.Log($"Room {gameObject.name}: Starting disable delay");
        yield return new WaitForSeconds(time);
        SetLockState = false;
        DisableInterfaces();
    }

    private void EnableInterfaces()
    {
        if (roomState == RoomState.COMPLETED)
        {
            //Debug.Log($"Room {gameObject.name}: Attempt to enable already completed room blocked");
            return;
        }

        //Debug.Log($"Room {gameObject.name}: Enabling interfaces");
        SetRoomContentsEnableState(true);
        hasPlayedCompletionSound = false;
        roomState = RoomState.ACTIVE;
    }

    private void DisableInterfaces()
    {
        //Debug.Log($"Room {gameObject.name}: Disabling interfaces");
        SetRoomContentsEnableState(false);
        roomState = RoomState.COMPLETED;
    }

    private bool ItemsCompleted()
    {
        bool allCompleted = true;
        bool allDestroyed = true;

        for (int i = 0; i < roomObjects.Length; i++)
        {
            if (roomObjects[i] != null)
            {
                allDestroyed = false;
                if (!roomObjects[i].ObjectComplete)
                {
                    allCompleted = false;
                    break;
                }
            }
        }

        bool completed = allCompleted || allDestroyed;

        if (completed && !hasPlayedCompletionSound && roomState == RoomState.ACTIVE)
        {
            if (SoundManager.Instance != null)
            {
                //Debug.Log($"Room {gameObject.name}: Playing completion sound");
                SoundManager.Instance.OnDoorStateChanged();
                hasPlayedCompletionSound = true;
            }
        }

        return completed;
    }

    private void SetRoomContentsEnableState(bool state)
    {
        //Debug.Log($"Room {gameObject.name}: Setting contents state to {state}");
        for (int i = 0; i < roomObjects.Length; i++)
        {
            if (roomObjects[i] != null)
            {
                if (state)
                    ((IRoomObject)roomObjects[i]).AwakenObject();
                else
                    ((IRoomObject)roomObjects[i]).SleepObject();
            }
        }
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        roomDetector.Draw();
    }
    #endregion
}
