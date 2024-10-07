#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

namespace Purrcifer.FloorGeneration
{ 
    public class StartRoomDecorator : FloorDecorator
    {
        public override bool Decorate(ref FloorPlan plan)
        {
            plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)MapIntMarkers.START;
            return true;
        }
    }
}