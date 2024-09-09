#define TOP_LEVEL_DEBUG
#undef TOP_LEVEL_DEBUG

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{
    #region Map Generation. 

    #region Abstract Base. 
    public abstract class MapGenerationEvent
    {
        public abstract bool Complete { get; internal set; }

        public abstract FloorPlan Plan { get; internal set; }

        public abstract IEnumerator GenerationEvent(FloorData data, FloorPlan plan);

        public abstract void Reset();
    }
    #endregion

    public class DrunkenWalk : MapGenerationEvent
    {
        public override bool Complete
        {
            get;
            internal set;
        } = false;

        public override FloorPlan Plan
        {
            get;
            internal set;
        }

        public override IEnumerator GenerationEvent(FloorData data, FloorPlan plan)
        {
            Queue<Vector2Int> roomQueue;

#if TOP_LEVEL_DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            //Generate the plan. 
            Plan = plan;

            //Create the room queue. 
            roomQueue = new Queue<Vector2Int>();
            roomQueue.Enqueue(new Vector2Int(data.floorWidth / 2, data.floorHeight / 2));

            //Create temp storage variable. 
            Vector2Int roomPos;

            //Generates the core of the map. 
            while (Plan.roomCount < data.roomCountMin)
            {
                roomPos = (roomQueue.Count > 0) ? roomQueue.Dequeue() : plan.GetRandomRoom();
                //Generate room connections.

                //Get random neighbouring cell. 
                Vector2Int n = Plan.GetAdjacentCells(roomPos.x, roomPos.y)[UnityEngine.Random.Range(0, 4)];

                //Attempt room generation. 
                if (plan.WithinRange(n.x, n.y))
                {
                    Plan[n.x, n.y] = 1;
                    Plan.roomCount++;
                    roomQueue.Enqueue(new Vector2Int(n.x, n.y));
                }

                //Delay
                yield return new WaitForEndOfFrame();
            }
#if TOP_LEVEL_DEBUG
            UnityEngine.Debug.Log("MapGenEvent: Time taken to generate base map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");
            stopwatch.Stop();
            Plan.Print();
#endif
            Complete = true;
        }

        public override void Reset()
        {
            Complete = false;
            Plan = null;
        }
    }

    public class EndPointAdditions : MapGenerationEvent
    {
        public override bool Complete
        {
            get;
            internal set;
        } = false;

        public override FloorPlan Plan
        {
            get;
            internal set;
        }

        public override IEnumerator GenerationEvent(FloorData data, FloorPlan plan)
        {
#if TOP_LEVEL_DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            Plan = plan;
            bool triedEndpoint = false;

            //Summate if there are enough end points.
            Plan.CacheEndpoints();

            while (Plan.EndPoints.Length < 3)
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

#if TOP_LEVEL_DEBUG
            Plan.Print();
            stopwatch.Stop();
            UnityEngine.Debug.Log("MapGenEvent: Time taken to generate extra endpoints map: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.");
#endif

            Complete = true;
        }

        private bool Intersect()
        {
            Vector2Int[] _endPoints = Plan.EndPoints;
            bool generated = false;

            for (int i = 0; i < _endPoints.Length; i++)
            {
                if (GenerateIntersectRooms(Plan, _endPoints[i].x, _endPoints[i].y))
                {
                    Plan.CacheEndpoints();
                    generated = true;
                    break;
                }
            }

            return generated;
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

        public override void Reset()
        {
            Complete = false;
            Plan = null;
        }
    }

#endregion

    #region Decorators. 
    public abstract class FloorDecorator
    {
        public abstract bool Decorate(ref FloorPlan plan);
    }

    public class StartRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)MapIntMarkers.START;
            return true;
        }
    }

    public class BossRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            int startMarker = (int)MapIntMarkers.START;
            Vector2Int[] endpoints = plan.EndPoints;
            Vector2Int[] neighbors;

            foreach (Vector2Int endpoint in endpoints)
            {
                neighbors = plan.GetAdjacentCells(endpoint.x, endpoint.y);

                if (plan.GetMarkCount(neighbors, startMarker) == 0
                    && plan[endpoint] != startMarker)
                {
                    plan[endpoint.x, endpoint.y] = (int)MapIntMarkers.BOSS;
                    plan.CacheEndpoints();
                    return true;
                }
            }
            return false;
        }
    }

    public class TreasureRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            int startMarker = (int)MapIntMarkers.START;
            int bossMarker = (int)MapIntMarkers.BOSS;
            Vector2Int[] endpoints = plan.EndPoints;
            Vector2Int[] neighbors;

            foreach (Vector2Int endpoint in endpoints)
            {
                neighbors = plan.GetAdjacentCells(endpoint.x, endpoint.y);

                if (plan.GetMarkCount(neighbors, bossMarker) == 0 &&
                    plan[endpoint] != startMarker &&
                    plan[endpoint] != bossMarker)
                {
                    plan[endpoint.x, endpoint.y] = (int)MapIntMarkers.TREASURE;
                    plan.CacheEndpoints();
                    return true;
                }
            }
            return false;
        }
    }

    public class HiddenRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            List<Vector2Int> rooms = plan.GetTypeMark(0);
            List<Vector2Int> roomsWithAdj = plan.SortByAdjacency(rooms, 3);

            if (roomsWithAdj.Count > 0)
            {
                plan[roomsWithAdj[0]] = (int)MapIntMarkers.HIDDEN_ROOM;
                return true;
            }

            return false;
        }
    }
    #endregion

    public class FloorGenerationHandler : MonoBehaviour
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

        public bool baseMapGenerated = false;
        public bool decorationGenerated = false;
        public bool objectConversionGenerated = false;

        public void GenerateBaseMap(FloorData data)
        {
            foreach (MapGenerationEvent item in events)
            {
                item.Reset();
            }

            StartCoroutine(GenerateMap(data));
        }

        private IEnumerator GenerateMap(FloorData data)
        {
            FloorPlan plan = new FloorPlan(data);
            for (int i = 0; i < events.Count; i++)
            {
                StartCoroutine(events[i].GenerationEvent(data, plan));
                while (!events[i].Complete)
                {
                    yield return new WaitForSeconds(0.05F);
                }

                plan = events[i].Plan;
            }

            if (plan.GetRoomCount() > data.roomCountMin && plan.GetRoomCount() < data.roomCountMax)
            {
                baseMapGenerated = true;
                StartCoroutine(DecorateMap(data, plan));
            }
            else
            {
                GenerateBaseMap(data);
            }

        }

        private IEnumerator DecorateMap(FloorData data, FloorPlan plan)
        {
            FloorPlan _plan = plan;
            int successCount = 0;

            for (int i = 0; i < decorators.Count; i++)
            {
                successCount += decorators[i].Decorate(ref _plan) ? 1 : 0;
                yield return new WaitForSeconds(0.05F);
            }

#if TOP_LEVEL_DEBUG
            plan.Print();
            if (successCount == decorators.Count)
                UnityEngine.Debug.LogAssertion("MapDecorators: Decoration Event Successful!");
            else
                UnityEngine.Debug.LogError("MapDecorators: Decoration Event Failed!");
#endif

            if (successCount == decorators.Count)
            {
                decorationGenerated = true;
                GameManager.FloorPlan = _plan;
            }
            else
            {
                GenerateBaseMap(data);
            }
        }
    }
}