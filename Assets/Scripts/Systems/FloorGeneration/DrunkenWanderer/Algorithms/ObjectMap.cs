using JetBrains.Annotations;
using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public static class ObjectGenHelper
{
    public static class RoomMappingConversions
    {
        public static Dictionary<MapIntMarkers, WallType> map =
            new Dictionary<MapIntMarkers, WallType>() {
            { MapIntMarkers.NONE, WallType.NONE },
            { MapIntMarkers.ROOM, WallType.WALL },
            { MapIntMarkers.START, WallType.WALL },
            { MapIntMarkers.BOSS, WallType.WALL },
            { MapIntMarkers.TREASURE, WallType.WALL },
            { MapIntMarkers.HIDDEN_ROOM, WallType.HIDDEN_ROOM },
            };
    }

    /// <summary>
    /// Generates an object within the map. 
    /// </summary>
    /// <param name="marker"> The marker type representing the object. </param>
    /// <param name="x"> The x coordinate of the object. </param>
    /// <param name="y"> The y coordinate of the object. </param>
    public static GameObject GetObjectRef(MapIntMarkers marker)
    {
        switch (marker)
        {
            case MapIntMarkers.NONE:
                return null;
            case MapIntMarkers.BOSS:
                return MasterTree.Instance.BossRoomTree.GetRandomPrefab(false);
            case MapIntMarkers.START:
                return MasterTree.Instance.StartRoomTree.GetRandomPrefab(false);
            case MapIntMarkers.ROOM:
                return MasterTree.Instance.NormalRoomTree.GetRandomPrefab(false);
            case MapIntMarkers.TREASURE:
                return MasterTree.Instance.TreasureRoomTree.GetRandomPrefab(false);
            case MapIntMarkers.HIDDEN_ROOM:
                return MasterTree.Instance.HiddenRoomTree.GetRandomPrefab(false);
            default:
                return null;
        }
    }
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
        roomSizeWidth = DefaultRoomData.DEFAULT_WIDTH;
        roomSizeHeight = DefaultRoomData.DEFAULT_HEIGHT;
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
        BuildRoom(x, y, marker);
    }

    /// <summary>
    /// Builds a room within world space and adds it to the object map.
    /// </summary>
    /// <param name="prefab"> The prefab to create. </param>
    /// <param name="x"> The x coordinate of the object. </param>
    /// <param name="y"> The y coordinate of the object. </param>
    private void BuildRoom(int x, int y, int marker)
    {
        GameObject prefab = ObjectGenHelper.GetObjectRef((MapIntMarkers)marker);
        
        if (prefab == null)
            return;
        
        this[x, y] = GameObject.Instantiate(prefab);

        WallType wallType = ObjectGenHelper.RoomMappingConversions.map[(MapIntMarkers)marker];
        Debug.Log(wallType.ToString());
        this[x, y].gameObject.GetComponent<RoomController>().MarkerType = wallType;
        this[x, y].name = this[x, y].name + "[" + x + ", " + y + "]";
        //Set the position of the object in world space. 
        this[x, y].transform.position = 
            new Vector3(initialPosition.x + x * roomSizeWidth, 0, initialPosition.y - y * roomSizeHeight);
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

        RoomController _roomCTRLR = _room.GetComponent<RoomController>();

        if (_roomCTRLR == null)
            return;

        DoorSetup(_roomCTRLR, x + 1, y, WallDirection.RIGHT, WallDirection.LEFT);
        DoorSetup(_roomCTRLR, x - 1, y, WallDirection.LEFT, WallDirection.RIGHT);
        DoorSetup(_roomCTRLR, x, y - 1, WallDirection.UP, WallDirection.DOWN);
        DoorSetup(_roomCTRLR, x, y + 1, WallDirection.DOWN, WallDirection.UP);

        void DoorSetup(RoomController ctrllr, int x, int y, WallDirection aOp, WallDirection bOp)
        {
            if (this[x, y] == null)
                return;

            //Check if are normal mappings. 
            //Else cover other mappings.
            // ->> Check for hidden room, if is hidden room, pass to the member that isn't a hidden room. 

            RoomController bController = this[x, y].GetComponent<RoomController>();
            WallType markerA = ctrllr.MarkerType;
            WallType markerB = bController.MarkerType;

            if((int)markerA == 3 | (int)markerB == 3)
            {
                markerA = markerB = (WallType)3;
            }
            else
            {
                markerA = markerB = WallType.DOOR;
            }

            ctrllr.SetRoomState(aOp, markerA);
            bController.SetRoomState(bOp, markerB);
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
