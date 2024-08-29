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

    private Vector2Int FloorCenter => new Vector2Int(Width / 2, Height / 2);

    public FloorPlan(FloorData data)
    {
        roomCount = 0;
        plan = new int[data.floorWidth, data.floorHeight];
        this[FloorCenter] = 1;
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

    public void SetMark(int x, int y)
    {
        plan[x, y] = 1;
        roomCount++;
    }

    public bool WithinRange(int x, int y)
    {
        return (plan[x, y] != -1) && (plan[x, y] != 1);
    }

    public Vector2Int GetRandomRoom()
    {
        List<Vector2Int> _rooms = GetTypeMark(1);
        return _rooms[UnityEngine.Random.Range(0, _rooms.Count)];
    }

    int SumCellsAdj(int i, int j)
    {
        int count = 0;
        count += (this[i + 1, j] != 0 && this[i + 1, j] != -1) ? 1 : 0;
        count += (this[i - 1, j] != 0 && this[i - 1, j] != -1) ? 1 : 0;
        count += (this[i, j + 1] != 0 && this[i, j + 1] != -1) ? 1 : 0;
        count += (this[i, j - 1] != 0 && this[i, j - 1] != -1) ? 1 : 0;
        return count;
    }

    public void Print() => FloorPlan.Print2DArray(plan);

    public int GetRoomCount()
    {
        int count = 0; 
        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for(int j = 0; j < plan.GetLength(1); j++)
            {
                if(plan[i, j] == 1)
                {
                    count++; 
                }
            }
        }

        return count;
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
        get
        {
            if(x > MIN && y > MIN && x < Width && y < Height)
            {
                return plan[x, y];
            }
            return -1; 
        }

        set => plan[x, y] = value;
    }

    private int this[Vector2Int v]
    {
        get
        {
            if (v.x > MIN && v.y > MIN && v.x < Width && v.y < Height)
            {
                return plan[v.x, v.y];
            }
            return -1;
        }

        set => plan[v.x, v.y] = value;
    }
}
