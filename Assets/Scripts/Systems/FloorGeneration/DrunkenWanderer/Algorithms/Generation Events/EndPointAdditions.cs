#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using FloorGeneration;
using System.Collections;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{

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
            Plan = plan;
            bool triedEndpoint = false;

            //Summate if there are enough end points.
            Plan.CacheEndpoints();

            while (Plan.EndPoints.Length < data.numberOfEndpoints)
            {
                Vector2Int randPos = Helpers_FloorPlan.GetRandomRoom(plan);

                //add more endpoints: 
                if (triedEndpoint == false)
                {
                    Vector2Int[] _endPoints = plan.EndPoints;

                    for (int i = 0; i < _endPoints.Length; i++)
                    {
                        if (GenerateIntersectRooms(plan, _endPoints[i].x, _endPoints[i].y))
                        {
                            plan.CacheEndpoints();
                            if (plan.EndPoints.Length <= data.numberOfEndpoints)
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
                    randPos = Helpers_FloorPlan.GetRandomRoom(plan);
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
            Vector2Int[] neighbours = Helpers_FloorPlan.GetAdjacentCells(plan, x, y);
            int random = UnityEngine.Random.Range(0, 4);
            if (plan[neighbours[random]] != -1)
                plan[neighbours[random]] = 1;
        }

        private bool GenerateIntersectRooms(FloorPlan plan, int x, int y)
        {
            Vector2Int[] n = Helpers_FloorPlan.GetAdjacentCells(plan, x, y);

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
}