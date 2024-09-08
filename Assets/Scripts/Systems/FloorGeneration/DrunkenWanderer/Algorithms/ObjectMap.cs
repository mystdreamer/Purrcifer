using UnityEngine;
using Purrcifer.Data.Defaults;

/// <summary>
/// Enums representing a door opening operation. 
/// </summary>
public enum WallDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

/// <summary>
/// Class used to convert the FloorMap int map into an object map. 
/// </summary>
public class ObjectMap
{
    /// <summary>
    /// Const representing the minimum bounds value.
    /// </summary>
    private const int MIN = -1;

    /// <summary>
    /// The map held by the game. 
    /// </summary>
    public GameObject[,] objectMap;

    /// <summary>
    /// The width per room. 
    /// </summary>
    public float roomSizeWidth;

    /// <summary>
    /// The height per room. 
    /// </summary>
    public float roomSizeHeight;

    /// <summary>
    /// The initial position defined by the map. 
    /// </summary>
    public Vector3 initialPosition;

    /// <summary>
    /// Provides the width of the map. 
    /// </summary>
    public int Width => objectMap.GetLength(0);

    /// <summary>
    /// Provides the height of the map. 
    /// </summary>
    public int Height => objectMap.GetLength(1);

    /// <summary>
    /// CTOR. 
    /// </summary>
    /// <param name="data"> The data instance used to build the floor plan. </param>
    /// <param name="plan"> The FloorPlan representing the world map. </param>
    public ObjectMap(FloorData data, FloorPlan plan)
    {
        initialPosition = new Vector3(data.initialX, data.initialY);
        roomSizeWidth = RoomDataDefaults.ROOM_WIDTH;
        roomSizeHeight = RoomDataDefaults.ROOM_HEIGHT;
        objectMap = new GameObject[plan.plan.GetLength(0), plan.plan.GetLength(1)];
    }

    /// <summary>
    /// Generates an object within the map. 
    /// </summary>
    /// <param name="marker"> The marker type representing the object. </param>
    /// <param name="x"> The x coordinate of the object. </param>
    /// <param name="y"> The y coordinate of the object. </param>
    public void GenerateObject(int marker, int x, int y)
    {
        switch (marker)
        {
            case (int)MapIntMarkers.NONE:
                break;
            case (int)MapIntMarkers.BOSS:
                BuildRoom(MasterTree.Instance.BossRoomTree.GetRandomPrefab(false), x, y);
                break;
            case (int)MapIntMarkers.START:
                BuildRoom(MasterTree.Instance.StartRoomTree.GetRandomPrefab(false), x, y);
                break;
            case (int)MapIntMarkers.ROOM:
                BuildRoom(MasterTree.Instance.NormalRoomTree.GetRandomPrefab(false), x, y);
                break;
            case (int)MapIntMarkers.TREASURE:
                BuildRoom(MasterTree.Instance.TreasureRoomTree.GetRandomPrefab(false), x, y);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Builds a room within world space and adds it to the object map.
    /// </summary>
    /// <param name="prefab"> The prefab to create. </param>
    /// <param name="x"> The x coordinate of the object. </param>
    /// <param name="y"> The y coordinate of the object. </param>
    private void BuildRoom(GameObject prefab, int x, int y)
    {
        this[x, y] = GameObject.Instantiate(prefab);
        this[x, y].name = this[x, y].name + "[" + x + ", " + y + "]";
        //Set the position of the object in world space. 
        this[x, y].transform.position = new Vector3(initialPosition.x + x * roomSizeWidth, 0, initialPosition.y - y * roomSizeHeight);
    }

    /// <summary>
    /// Enables doors around a give cell. 
    /// </summary>
    /// <param name="x"> The x coord of the cell. </param>
    /// <param name="y"> The y coord of the cell. </param>
    public void EnableDoors(int x, int y)
    {
        //Cache neighbour marks.
        GameObject _room = this[x, y];

        if (_room == null)
            return;
        RoomWallController _roomCTRLR = _room.GetComponent<RoomWallController>();

        if (_roomCTRLR == null)
            return;

        DoorSetup(_roomCTRLR, x + 1, y, WallDirection.RIGHT, WallDirection.LEFT);
        DoorSetup(_roomCTRLR, x - 1, y, WallDirection.LEFT, WallDirection.RIGHT);
        DoorSetup(_roomCTRLR, x, y - 1, WallDirection.UP, WallDirection.DOWN);
        DoorSetup(_roomCTRLR, x, y + 1, WallDirection.DOWN, WallDirection.UP);

        void DoorSetup(RoomWallController controller, int x, int y, WallDirection aOp, WallDirection bOp)
        {
            if (this[x, y] != null)
            {
                controller.SetDoorState(aOp);
                this[x, y].GetComponent<RoomWallController>().SetDoorState(bOp);
            }
        }
    }

    /// <summary>
    /// Index operator overload [].
    /// Assigns/reads from a provided cell address. 
    /// </summary>
    /// <param name="x"> The x coordinate of the cell. </param>
    /// <param name="y"> The y coordinate of the cell. </param>
    /// <returns> The cell address's mark if defined, otherwise -1. </returns>
    public GameObject this[int x, int y]
    {
        get => (x > MIN && y > MIN && x < Width && y < Height) ? this.objectMap[x, y] : null;
        set
        {
            if (x > MIN && y > MIN && x < Width && y < Height)
                this.objectMap[x, y] = value;
        }
    }

    /// <summary>
    /// Index operator overload [].
    /// Assigns/reads from a provided cell address. 
    /// </summary>
    /// <param name="v"> The vector position to read/assign to. </param>
    /// <returns> The cell address's mark if defined, otherwise -1. </returns>
    public GameObject this[Vector2Int v]
    {
        get => (v.x > MIN && v.y > MIN && v.x < Width && v.y < Height) ? this[v.x, v.y] : null;
        set
        {
            if (v.x > MIN && v.y > MIN && v.x < Width && v.y < Height)
                this[v.x, v.y] = value;
        }
    }
}
