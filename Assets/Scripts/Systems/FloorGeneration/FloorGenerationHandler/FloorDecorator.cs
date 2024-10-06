#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

namespace Purrcifer.FloorGeneration
{ 
    public abstract class FloorDecorator
    {
        public abstract bool Decorate(ref FloorPlan plan);
    }
}