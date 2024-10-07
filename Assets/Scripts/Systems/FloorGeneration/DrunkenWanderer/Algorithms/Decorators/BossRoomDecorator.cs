#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using FloorGeneration;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{
    public class BossRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            int startMarker = (int)MapIntMarkers.START;
            Vector2Int[] endpoints = plan.EndPoints;
            Vector2Int[] neighbors;

            foreach (Vector2Int endpoint in endpoints)
            {
                neighbors = Helpers_FloorPlan.GetAdjacentCells(plan, endpoint.x, endpoint.y);

                if (Helpers_FloorPlan.GetMarkCount(plan, neighbors, startMarker) == 0
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
}