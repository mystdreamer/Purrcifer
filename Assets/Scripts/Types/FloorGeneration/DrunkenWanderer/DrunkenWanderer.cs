using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DrunkenWanderer
{
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    public FloorPlan plan;
    public bool wanderComplete = false;
    public bool extraComplete = false;

    public IEnumerator Wander(FloorData data)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        plan = new FloorPlan(data);
        roomQueue.Enqueue(new Vector2Int(data.floorWidth / 2, data.floorHeight / 2));
        Vector2Int roomPos;

        //Generates the core of the map. 
        while (plan.roomCount < data.roomCountMin)
        {
            roomPos = (roomQueue.Count > 0) ? roomQueue.Dequeue() : plan.GetRandomRoom();

            //Generate room connections. 

            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0:
                    AttemptRoomGen(ref plan, roomPos.x, roomPos.y - 1);
                    break;
                case 1:
                    AttemptRoomGen(ref plan, roomPos.x, roomPos.y + 1);
                    break;
                case 2:
                    AttemptRoomGen(ref plan, roomPos.x + 1, roomPos.y);
                    break;
                case 3:
                    AttemptRoomGen(ref plan, roomPos.x - 1, roomPos.y);
                    break;
            }

            yield return new WaitForEndOfFrame();
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log("Time taken to generate base map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");
        FloorPlan.Print2DArray(plan.plan);
        wanderComplete = true;
    }

    public IEnumerator AddExtraEndpoints(FloorData data)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        //Summate if there are enough end points.
        plan.CacheEndpoints();

        bool triedEndpoint = false;

        while (plan.EndPoints.Length < 3)
        {
            Vector2Int randPos = plan.GetRandomRoom();

            //add more endpoints: 
            if (triedEndpoint == false)
            {
                Vector2Int[] _endPoints = plan.EndPoints;

                for (int i = 0; i < _endPoints.Length; i++)
                {
                    if (GenerateIntersectRooms(plan, _endPoints[i].x, _endPoints[i].y))
                    {
                        plan.CacheEndpoints();
                        if (plan.EndPoints.Length >= 3)
                        {
                            break;
                        }
                    }
                    yield return new WaitForEndOfFrame();
                }

                triedEndpoint = true;
            }
            else
            {
                randPos = plan.GetRandomRoom();
                if (!GenerateIntersectRooms(plan, randPos.x, randPos.y))
                {
                    GenerateRoom(plan, randPos.x, randPos.y);
                }
            }

            plan.CacheEndpoints();
            yield return new WaitForEndOfFrame();
        }

        FloorPlan.Print2DArray(plan.plan);
        stopwatch.Stop();
        UnityEngine.Debug.Log("Time taken to generate extra endpoints map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");

        extraComplete = true;
    }

    private void GenerateRoom(FloorPlan plan, int x, int y)
    {
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                if (!plan.WithinRange(x, y - 1))
                    return;
                plan.SetMark(x, y - 1);
                break;
            case 1:
                if (!plan.WithinRange(x, y + 1))
                    return;
                plan.SetMark(x, y + 1);
                break;
            case 2:
                if (!plan.WithinRange(x - 1, y))
                    return;
                plan.SetMark(x - 1, y);
                break;
            case 3:
                if (!plan.WithinRange(x + 1, y))
                    return;
                plan.SetMark(x + 1, y);
                break;
        }
    }

    private bool GenerateIntersectRooms(FloorPlan plan, int x, int y)
    {
        if (plan.WithinRange(x - 1, y) && plan.WithinRange(x + 1, y))
        {
            plan.SetMark(x - 1, y);
            plan.SetMark(x + 1, y);
            return true;
        }
        else if (plan.WithinRange(x, y - 1) && plan.WithinRange(x, y + 1))
        {
            plan.SetMark(x, y - 1);
            plan.SetMark(x, y + 1);
            return true;
        }
        return false;
    }

    private void AttemptRoomGen(ref FloorPlan plan, int x, int y)
    {
        if (!plan.WithinRange(x, y))
            return;
        plan.SetMark(x, y);
        roomQueue.Enqueue(new Vector2Int(x, y));
    }
}
