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

            //Get random neighbouring cell. 
            Vector2Int n = plan.GetAdjacentCells(roomPos.x, roomPos.y)[UnityEngine.Random.Range(0, 4)];

            //Attempt room generation. 
            AttemptRoomGen(ref plan, n.x, n.y);

            //Delay
            yield return new WaitForEndOfFrame();
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log("Time taken to generate base map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");
        plan.Print();
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

        plan.Print();
        stopwatch.Stop();
        UnityEngine.Debug.Log("Time taken to generate extra endpoints map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");

        extraComplete = true;
    }

    private void GenerateRoom(FloorPlan plan, int x, int y)
    {
        Vector2Int[] neighbours = plan.GetAdjacentCells(x, y);
        int random = UnityEngine.Random.Range(0, 4);
        if (plan[neighbours[random]] != -1)
            plan[neighbours[random]] = 1;
    }

    private bool GenerateIntersectRooms(FloorPlan plan, int x, int y)
    {
        Vector2Int[] n = plan.GetAdjacentCells(x, y);

        for (int i = 0; i < 2; i++)
        {
            if (plan.WithinRange(n[0].x, n[0].y) && plan.WithinRange(n[1].x, n[1].y))
            {
                plan[n[i]] = 1;
                plan[n[i + 1]] = 1;
                return true;
            }
        }
        return false;
    }

    private void AttemptRoomGen(ref FloorPlan plan, int x, int y)
    {
        if (plan.WithinRange(x, y))
        {
            plan.SetMark(x, y);
            roomQueue.Enqueue(new Vector2Int(x, y));
        }
    }
}
