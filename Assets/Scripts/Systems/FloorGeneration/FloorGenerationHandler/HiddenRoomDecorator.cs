#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using FloorGeneration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{
    public class HiddenRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            Vector2Int[] rooms = Helpers_FloorPlan.GetMarksWithType(plan, 0);
            Vector2Int[] roomsWithAdj = Helpers_FloorPlan.SortByAdjacency(plan, rooms, 2);

            if (roomsWithAdj.Length > 0)
            {
                plan[roomsWithAdj[0]] = (int)MapIntMarkers.HIDDEN_ROOM;
                return true;
            }

            return false;
        }
    }
}