#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using System.Collections;


namespace Purrcifer.FloorGeneration
{ 
    public abstract class MapGenerationEvent
    {
        public abstract bool Complete { get; internal set; }

        public abstract FloorPlan Plan { get; internal set; }

        public abstract IEnumerator GenerationEvent(FloorData data, FloorPlan plan);

        public abstract void Reset();
    }
}