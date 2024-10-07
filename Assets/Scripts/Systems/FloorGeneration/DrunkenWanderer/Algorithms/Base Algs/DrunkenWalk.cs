#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using FloorGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{

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
                roomPos = (roomQueue.Count > 0) ? roomQueue.Dequeue() : Helpers_FloorPlan.GetRandomRoom(plan);
                //Generate room connections.

                //Get random neighbouring cell. 
                Vector2Int n = Helpers_FloorPlan.GetAdjacentCells(plan, roomPos.x, roomPos.y)[UnityEngine.Random.Range(0, 4)];

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
}