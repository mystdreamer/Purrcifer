using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorPlan
{
    private const int MIN = -1;
    public int[,] plan;
    public int roomCount;
    public Vector2Int floorCenter;
    private Vector2Int[] endPoints;

    public int Width => plan.GetLength(0);
    public int Height => plan.GetLength(1);
    public Vector2Int[] EndPoints => endPoints;

    public FloorPlan(FloorData data)
    {
        roomCount = 0;
        plan = new int[data.floorWidth, data.floorHeight];
        floorCenter = new Vector2Int(data.floorWidth / 2, data.floorHeight / 2);
        this[data.floorWidth / 2, data.floorHeight / 2] = 1;
    }

    public void CacheEndpoints()
    {
        List<Vector2Int> matched = GetTypeMark((int)MapIntMarkers.ROOM);
        endPoints = SortByAdjacency(matched, 1).ToArray();
    }

    public List<Vector2Int> SortByAdjacency(List<Vector2Int> values, int adjCount)
    {
        List<Vector2Int> resulting = new List<Vector2Int>();
        for (int i = 0; i < values.Count; i++)
        {
            if (SumCellsAdj(values[i].x, values[i].y) == adjCount)
                resulting.Add(values[i]);
        }
        return resulting;
    }

    public List<Vector2Int> GetTypeMark(int type)
    {
        List<Vector2Int> matched = new List<Vector2Int>();

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == type)
                {
                    matched.Add(new Vector2Int(i, j));
                }
            }
        }
        return matched;
    }

    public int GetMarkCount(Vector2Int[] points, int mark)
    {
        int count = 0;
        for (int i = 0; i < points.Length; i++)
            count += (this[points[i]] != mark) ? 0 : 1;
        return count;
    }

    public void SetMark(int x, int y)
    {
        plan[x, y] = 1;
        roomCount++;
    }

    public bool WithinRange(int x, int y)
    {
        return (this[x, y] != -1) && (this[x, y] != 1);
    }

    public Vector2Int GetRandomRoom()
    {
        List<Vector2Int> _rooms = GetTypeMark(1);
        return _rooms[UnityEngine.Random.Range(0, _rooms.Count)];
    }

    int SumCellsAdj(int i, int j)
    {
        Vector2Int[] neighbours = GetAdjacentCells(i, j);
        int count = 0;

        foreach (Vector2Int n in neighbours)
        {
            count += (this[n] != 0 && this[n] != -1) ? 1 : 0;
        }
        return count;
    }

    public void Print() => FloorPlan.Print2DArray(plan);

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

    public int this[int x, int y]
    {
        get => (x > MIN && y > MIN && x < Width && y < Height) ? plan[x, y] : -1;
        set
        {
            if (x > MIN && y > MIN && x < Width && y < Height)
                plan[x, y] = value;
        }
    }

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
