using JetBrains.Annotations;
using Purrcifer.Data.Defaults;
using Purrcifer.FloorGeneration.RoomResolution;
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
                return MasterTree.GetBossRoomPrefab;
            case MapIntMarkers.START:
                return MasterTree.GetStartRoomPrefab;
            case MapIntMarkers.ROOM:
                return MasterTree.GetNormalRoomPrefab;
            case MapIntMarkers.TREASURE:
                return MasterTree.GetTreasureRoomPrefab;
            case MapIntMarkers.HIDDEN_ROOM:
                return MasterTree.GetHiddenRoomPrefab;
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

    public FloorPlan plan;

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
        this.plan = plan;
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
        this[x, y] = RoomInstancing.BuildRoomObject(
            ((MapIntMarkers)marker),
            (int)initialPosition.x, (int)initialPosition.y,
            x, y, roomSizeWidth, roomSizeHeight);
    }

    /// <summary>
    /// Enables doors around a give cell. 
    /// </summary>
    /// <param name="x"> The x coord of the cell. </param>
    /// <param name="y"> The y coord of the cell. </param>
    public void EnableDoors(int x, int y)
    {
        ResolveRoomDoorRules.ResolveRoomDoors(plan, this, x, y);
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
