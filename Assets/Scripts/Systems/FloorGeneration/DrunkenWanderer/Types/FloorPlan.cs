using FloorGeneration;
using Purrcifer.FloorGeneration;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible for handling int maps. 
/// </summary>
[System.Serializable]
public class FloorPlan
{
    public List<MapGenerationEvent> events = new List<MapGenerationEvent>()
        {
            new DrunkenWalk(),
            new EndPointAdditions()
        };

    public List<FloorDecorator> decorators = new List<FloorDecorator>()
        {
            new StartRoomDecorator(),
            new BossRoomDecorator(),
            new TreasureRoomDecorator(),
            new HiddenRoomDecorator(),
        };

    /// <summary>
    /// The minimum value of the map. 
    /// </summary>
    private const int MIN = -1;

    /// <summary>
    /// The integer map representing the world.
    /// </summary>
    public int[,] plan;

    /// <summary>
    /// The current number of rooms within the map. 
    /// </summary>
    public int roomCount;

    /// <summary>
    /// The center position of the map. 
    /// </summary>
    public Vector2Int floorCenter;

    /// <summary>
    /// The cached list of endpoints. 
    /// </summary>
    private Vector2Int[] endPoints;

    /// <summary>
    /// Provides the width of the map. 
    /// </summary>
    public int Width => plan.GetLength(0);

    /// <summary>
    /// Provides the height of the map. 
    /// </summary>
    public int Height => plan.GetLength(1);

    public bool PlanValid => Helpers_FloorPlan.MapSizeValid(this, Width, Height);

    public Vector2Int[] GetNormalRooms => Helpers_FloorPlan.GetMarksWithType(this, (int)MapIntMarkers.ROOM);

    /// <summary>
    /// Returns an array containing all the endpoints in the level. 
    /// </summary>
    public Vector2Int[] EndPoints => endPoints;

    /// <summary>
    /// CTOR.
    /// </summary>
    /// <param name="data"> The floor data required for constructing the map. </param>
    public FloorPlan(FloorData data)
    {
        roomCount = 0;
        plan = new int[data.floorWidth, data.floorHeight];
        floorCenter = new Vector2Int(data.floorWidth / 2, data.floorHeight / 2);
        this[data.floorWidth / 2, data.floorHeight / 2] = 1;
    }

    /// <summary>
    /// Caches the endpoints within the level. 
    /// </summary>
    public void CacheEndpoints()
    {
        Vector2Int[] matched = Helpers_FloorPlan.GetMarksWithType(this, (int)MapIntMarkers.ROOM);
        endPoints = Helpers_FloorPlan.SortByAdjacency(this, matched.ToList(), 1).ToArray();
    }

    /// <summary>
    /// Returns if the mark is a real value and is not a default room. 
    /// </summary>
    /// <param name="x"> The x coord to check. </param>
    /// <param name="y"> The y coord to check. </param>
    /// <returns> True if within range, else false. </returns>
    public bool WithinRange(int x, int y) =>
        (this[x, y] != -1) && (this[x, y] != 1);

    /// <summary>
    /// Prints the map to the console. 
    /// </summary>
    public void Print() => Helpers_FloorPlan.Print2DArray(plan);

    #region Operators. 
    /// <summary>
    /// Index operator overload [].
    /// Assigns/reads from a provided cell address. 
    /// </summary>
    /// <param name="x"> The x coordinate of the cell. </param>
    /// <param name="y"> The y coordinate of the cell. </param>
    /// <returns> The cell address's mark if defined, otherwise -1. </returns>
    public int this[int x, int y]
    {
        get => (x > MIN && y > MIN && x < Width && y < Height) ? plan[x, y] : -1;
        set
        {
            if (x > MIN && y > MIN && x < Width && y < Height)
                plan[x, y] = value;
        }
    }

    /// <summary>
    /// Index operator overload [].
    /// Assigns/reads from a provided cell address. 
    /// </summary>
    /// <param name="v"> The vector position to read/assign to. </param>
    /// <returns> The cell address's mark if defined, otherwise -1. </returns>
    public int this[Vector2Int v]
    {
        get => (v.x > MIN && v.y > MIN && v.x < Width && v.y < Height) ? plan[v.x, v.y] : -1;
        set
        {
            if (v.x > MIN && v.y > MIN && v.x < Width && v.y < Height)
                plan[v.x, v.y] = value;
        }
    }
    #endregion
}
