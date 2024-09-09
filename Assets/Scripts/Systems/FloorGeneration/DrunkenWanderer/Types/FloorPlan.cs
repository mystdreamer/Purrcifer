using System.Collections.Generic;
using UnityEngine;

public static class HelperFloorPlan
{
    /// <summary>
    /// Get a list of positions associated with the given mark. 
    /// </summary>
    /// <param name="mark"> The mark type to retrieve, </param>
    /// <returns> List of Vector2Int positions with the provided mark. </returns>
    public static List<Vector2Int> GetTypeMark(FloorPlan plan, int mark)
    {
        List<Vector2Int> matched = new List<Vector2Int>();

        for (int i = 0; i < plan.plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.plan.GetLength(1); j++)
            {
                if (plan[i, j] == mark)
                {
                    matched.Add(new Vector2Int(i, j));
                }
            }
        }
        return matched;
    }
}

/// <summary>
/// Class responsible for handling int maps. 
/// </summary>
[System.Serializable]
public class FloorPlan
{
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

    public List<Vector2Int> GetNormalRooms => HelperFloorPlan.GetTypeMark(this, (int)MapIntMarkers.ROOM);

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
        List<Vector2Int> matched = GetTypeMark((int)MapIntMarkers.ROOM);
        endPoints = SortByAdjacency(matched, 1).ToArray();
    }

    /// <summary>
    /// Returns a list of cells that match the required adjacency. 
    /// </summary>
    /// <param name="values"> The values to check. </param>
    /// <param name="adjCount"> The number of cells adjacent to the list provided. </param>
    /// <returns> List of cells matching the required adjacency count. </returns>
    public List<Vector2Int> SortByAdjacency(List<Vector2Int> values, int adjCount)
    {
        List<Vector2Int> resulting = new List<Vector2Int>();
        for (int i = 0; i < values.Count; i++)
        {
            if (SumCellsAdjacent(values[i].x, values[i].y) == adjCount)
                resulting.Add(values[i]);
        }
        return resulting;
    }

    /// <summary>
    /// Get a list of positions associated with the given mark. 
    /// </summary>
    /// <param name="mark"> The mark type to retrieve, </param>
    /// <returns> List of Vector2Int positions with the provided mark. </returns>
    public List<Vector2Int> GetTypeMark(int mark)
    {
        List<Vector2Int> matched = new List<Vector2Int>();

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == mark)
                {
                    matched.Add(new Vector2Int(i, j));
                }
            }
        }
        return matched;
    }

    /// <summary>
    /// Returns the number of rooms with a given mark. 
    /// </summary>
    /// <param name="points"> The array to check. </param>
    /// <param name="mark"> The mark to compare with. </param>
    public int GetMarkCount(Vector2Int[] points, int mark)
    {
        int count = 0;
        for (int i = 0; i < points.Length; i++)
            count += (this[points[i]] != mark) ? 0 : 1;
        return count;
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
    /// Returns a random normal room from the map. 
    /// </summary>
    /// <returns> Vector2Int with the map coords of the room. </returns>
    public Vector2Int GetRandomRoom()
    {
        List<Vector2Int> _rooms = GetTypeMark(1);
        return _rooms[UnityEngine.Random.Range(0, _rooms.Count)];
    }

    /// <summary>
    /// Sums the number of rooms adjacent to the cell. 
    /// </summary>
    /// <param name="x"> The x coord of the cell. </param>
    /// <param name="y"> The y coord of the cell. </param>
    /// <returns> Integer number representing the cells attached.</returns>
    int SumCellsAdjacent(int x, int y)
    {
        Vector2Int[] neighbours = GetAdjacentCells(x, y);
        int count = 0;

        foreach (Vector2Int n in neighbours)
        {
            count += (this[n] != 0 && this[n] != -1) ? 1 : 0;
        }
        return count;
    }

    /// <summary>
    /// Prints the map to the console. 
    /// </summary>
    public void Print() => FloorPlan.Print2DArray(plan);

    /// <summary>
    /// Returns the total number of cells in the map. 
    /// </summary>
    public int GetRoomCount()
    {
        int count = 0;
        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == 1)
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Returns the positions of cells neighbouring the provided cell. 
    /// </summary>
    /// <param name="x"> The x position of the cell. </param>
    /// <param name="y"> The y position of the cell. </param>
    /// <returns> Vector2Int[] list containing the neighbouring addresses. </returns>
    public Vector2Int[] GetAdjacentCells(int x, int y)
    {
        return new Vector2Int[]
        {
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y),
            new Vector2Int(x, y + 1),
            new Vector2Int(x, y - 1)
        };
    }

    /// <summary>
    /// Static function for compiling the map into a string and printing to the console. 
    /// </summary>
    private static void Print2DArray<T>(T[,] matrix)
    {
        string outP = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                outP += (matrix[i, j] + "\t");
            }
            outP += "\n";
        }
        Debug.Log(outP);
    }

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
}
