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
        plan[floorCenter.x, floorCenter.y] = 1;
    }

    public void CacheEndpoints()
    {
        List<Vector2Int> matched = GetTypeMark((int)DecoratorMarkers.ROOM);
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

    public void SetMark(Vector2Int pos)
    {
        plan[pos.x, pos.y] = 1;
        roomCount++;
    }

    public void SetMark(int x, int y)
    {
        plan[x, y] = 1;
        roomCount++;
    }

    public void ChangeMark(Vector2Int pos, int marker)
    {
        plan[pos.x, pos.y] = marker;
    }

    public bool WithinRange(Vector2Int point)
    {
        return WithinRange(point.x, point.y);
    }

    public bool WithinRange(int x, int y)
    {
        return (x > MIN && y > MIN && x < plan.GetLength(0) && y < plan.GetLength(1)) && (plan[x, y] != 1);
    }

    public Vector2Int GetRandomRoom()
    {
        List<Vector2Int> _rooms = GetTypeMark(1);
        return _rooms[UnityEngine.Random.Range(0, _rooms.Count)];
    }

    public int GetRoomState(int x, int y)
    {
        if (x < 0 || y < 0 || x > Width || y > Height)
            return -1;
        return plan[x, y];
    }

    int SumCellsAdj(int i, int j)
    {
        int count = 0;
        count += CheckAdjCell(i + 1, j);
        count += CheckAdjCell(i - 1, j);
        count += CheckAdjCell(i, j + 1);
        count += CheckAdjCell(i, j - 1);
        return count;
    }

    int CheckAdjCell(int x, int y)
    {
        int state;
        if (x > plan.GetLength(0) - 1 | x < 0 | y > plan.GetLength(1) - 1 | y < 0)
            return 0;

        state = GetRoomState(x, y);
        if (state != 0)
            return 1;
        return 0;
    }

    public void Print()
    {
        FloorPlan.Print2DArray(plan);
    }

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

    public static void Print2DArray<T>(T[,] matrix)
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
}
