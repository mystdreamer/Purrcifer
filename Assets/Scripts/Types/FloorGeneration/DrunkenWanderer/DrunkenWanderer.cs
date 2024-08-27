using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Decorator;

[System.Serializable]
public struct FloorData
{
    public int roomCount;
    public int floorWidth;
    public int floorHeight;
    public int initialX, initialY;
    public Range rangeProbX;
    public float spawnOver;

    public bool SpawnChance => UnityEngine.Random.Range(rangeProbX.min, rangeProbX.max) > spawnOver;
}

[System.Serializable]
public class FloorPlan
{
    private const int MIN = -1;
    public int[,] plan;
    public int roomCount;
    public Vector2Int floorCenter;

    public int Width => plan.GetLength(0);
    public int Height => plan.GetLength(1);

    public FloorPlan(FloorData data)
    {
        roomCount = 0;
        plan = new int[data.floorWidth, data.floorHeight];
        floorCenter = new Vector2Int(data.floorWidth / 2, data.floorHeight / 2);
        plan[floorCenter.x, floorCenter.y] = 1;
    }

    public void SetMark(Vector2Int pos)
    {
        plan[pos.x, pos.y] = 1;
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
        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == 1)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return floorCenter;
    }

    public int GetRoomState(Vector2Int position)
    {
        if (!WithinRange(position))
            return -1;
        else
        {
            return plan[position.x, position.y];
        }
    }

    public int GetRoomState(int x, int y)
    {
        return plan[x, y];
    }

    public int[,] GetEndpointMap()
    {
        int[,] map = new int[plan.GetLength(0), plan.GetLength(1)];

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == 1)
                {
                    int result = SumCellsAdj(i, j);
                    if (result == 1)
                        map[i, j] = 1;
                }
            }
        }

        return map;
    }

    public int GetEndpointCount()
    {
        int count = 0;

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == 1)
                {
                    int result = SumCellsAdj(i, j);
                    if (result == 1)
                        count++;
                }
            }
        }

        return count;
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
        if (state == 1 || state == 2)
            return 1;
        return 0;
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

public class DrunkenWanderer
{
    private static Vector2Int up = new Vector2Int(0, 1);
    private static Vector2Int down = new Vector2Int(0, -1);
    private static Vector2Int right = new Vector2Int(1, 0);
    private static Vector2Int left = new Vector2Int(1, 0);

    public static FloorPlan GenerateFloorMap(FloorData mapData)
    {
        FloorPlan plan = new FloorPlan(mapData);
        Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
        roomQueue.Enqueue(new Vector2Int(mapData.floorWidth / 2, mapData.floorHeight / 2));
        int drunkEnergy = mapData.roomCount;
        Vector2Int roomPos;
        int direction;

        //Generates the core of the map. 
        while (plan.roomCount < mapData.roomCount)
        {
            if (roomQueue.Count > 0)
                roomPos = roomQueue.Dequeue();
            else
                roomPos = plan.GetRandomRoom();

            direction = UnityEngine.Random.Range(0, 4);

            //Generate room connections. 

            switch (direction)
            {
                case 0:
                    AttemptRoomGen(ref plan, ref roomQueue, roomPos + up, true, mapData);
                    break;
                case 1:
                    AttemptRoomGen(ref plan, ref roomQueue, roomPos + down, true, mapData);
                    break;
                case 2:
                    AttemptRoomGen(ref plan, ref roomQueue, roomPos + left, true, mapData);
                    break;
                case 3:
                    AttemptRoomGen(ref plan, ref roomQueue, roomPos + right, true, mapData);
                    break;
            }
        }

        //Summate if there are enough end points.
        int endpointCount = plan.GetEndpointCount();

        while (endpointCount < 3)
        {
            //add more endpoints: 
            Vector2Int randPos = plan.GetRandomRoom();
            GenerateRoom(plan, randPos);
            endpointCount = plan.GetEndpointCount();
        }

        return plan;
    }

    private static void GenerateRoom(FloorPlan plan, Vector2Int point)
    {
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                if (!plan.WithinRange(point + up))
                    return;
                plan.SetMark(point + up);
                break;
            case 1:
                if (!plan.WithinRange(point + down))
                    return;
                plan.SetMark(point + down);
                break;
            case 2:
                if (!plan.WithinRange(point + left))
                    return;
                plan.SetMark(point + left);
                break;
            case 3:
                if (!plan.WithinRange(point + right))
                    return;
                plan.SetMark(point + right);
                break;
        }
    }

    private static void AttemptRoomGen(ref FloorPlan plan, ref Queue<Vector2Int> queue, Vector2Int point, bool rand, FloorData data)
    {
        if (!plan.WithinRange(point))
            return;
        plan.SetMark(point);
        queue.Enqueue(point);
    }
}

public enum DecoratorMarkers
{
    NONE = 0,
    ROOM = 1,
    START = 2,
    BOSS = 3,
    TREASURE = 4
}

public class MapEndPoint
{
    public Vector2Int position;
    public NeighbourResults NeighbourResults;

    public MapEndPoint(Vector2Int pos, NeighbourResults results)
    {
        this.position = pos;
        this.NeighbourResults = results;
    }
}

public struct NeighbourResults
{
    public int upState;
    public int downState;
    public int leftState;
    public int rightState;
    public Vector2Int upIndex;
    public Vector2Int downIndex;
    public Vector2Int leftIndex;
    public Vector2Int rightIndex;

    public int GetNormalAttachedCount()
    {
        int count = 0;
        if (upState != (int)DecoratorMarkers.NONE)
            count++;
        if (downState != (int)DecoratorMarkers.NONE)
            count++;
        if (rightState != (int)DecoratorMarkers.NONE)
            count++;
        if (leftState != (int)DecoratorMarkers.NONE)
            count++;
        return count;
    }
}

public abstract class Decorator
{
    public abstract FloorPlan Decorate(FloorPlan plan, out bool success);
}

public class ApplyStart : Decorator
{
    public override FloorPlan Decorate(FloorPlan plan, out bool success)
    {
        plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)DecoratorMarkers.START;
        success = true;
        return plan;
    }
}

public class DefineExit : Decorator
{
    public override FloorPlan Decorate(FloorPlan plan, out bool success)
    {
        int epCount = plan.GetEndpointCount();
        if (epCount == 0)
        {
            success = false;
            return plan;
        }
        else
        {
            int[,] endpoints = plan.GetEndpointMap();
            for (int i = 0; i < endpoints.GetLength(0); i++)
            {
                for (int j = 0; j < endpoints.GetLength(1); j++)
                {
                    if (endpoints[i, j] == 1)
                    {
                        plan.plan[i, j] = (int)DecoratorMarkers.BOSS;
                        success = true;
                        return plan;
                    }
                }
            }
        }
        success = false;
        return plan;
    }
}