#define TOP_LEVEL_DEBUG
//#undef TOP_LEVEL_DEBUG

using System.Collections;
using System.Diagnostics;
using UnityEngine;


namespace Purrcifer.FloorGeneration
{

    public class FloorGenerationHandler : MonoBehaviour
    {
        public bool baseMapGenerated = false;
        public bool decorationGenerated = false;
        public bool objectConversionGenerated = false;

        public void GenerateBaseMap(FloorData data)
        {
            StartCoroutine(GenerateMap(data));
        }

        private IEnumerator GenerateMap(FloorData data)
        {
            FloorPlan plan = new FloorPlan(data);
            for (int i = 0; i < plan.events.Count; i++)
            {
                StartCoroutine(plan.events[i].GenerationEvent(data, plan));
                while (!plan.events[i].Complete)
                    yield return new WaitForSeconds(0.01F);

                plan = plan.events[i].Plan;
            }

            if (!plan.PlanValid)
            {
                baseMapGenerated = true;
                StartCoroutine(DecorateMap(data, plan));
            }
            else
                GenerateBaseMap(data);
        }

        private IEnumerator DecorateMap(FloorData data, FloorPlan plan)
        {
            FloorPlan _plan = plan;
            int successCount = 0;

            for (int i = 0; i < plan.decorators.Count; i++)
            {
                successCount += plan.decorators[i].Decorate(ref _plan) ? 1 : 0;
                yield return new WaitForSeconds(0.05F);
            }

#if TOP_LEVEL_DEBUG
            plan.Print();
            if (successCount == plan.decorators.Count)
                UnityEngine.Debug.LogAssertion("MapDecorators: Decoration Event Successful!");
            else
                UnityEngine.Debug.LogError("MapDecorators: Decoration Event Failed!");
#endif

            if (successCount == plan.decorators.Count)
            {
                decorationGenerated = true;
                GameManager.Instance.FloorPlan = _plan;
            }
            else
                GenerateBaseMap(data);
        }
    }
}