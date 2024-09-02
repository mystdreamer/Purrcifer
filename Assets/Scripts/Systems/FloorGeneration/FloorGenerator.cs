using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloorGeneration
{
    public class FloorGenerator : MonoBehaviour
    {
        #region Int Map Generation. 
        private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

        private void GenerateRandomMap(FloorData data)
        {
            StartCoroutine(DrunkenWanderer(data));
        }

        private IEnumerator DrunkenWanderer(FloorData data)
        {
            DrunkenWanderer wander = new DrunkenWanderer();
            StartCoroutine(wander.Wander(data));

            while (!wander.wanderComplete)
                yield return new WaitForEndOfFrame();

            StartCoroutine(wander.AddExtraEndpoints(data));

            while (!wander.extraComplete)
                yield return new WaitForEndOfFrame();


            if (wander.plan.GetRoomCount() > data.roomCountMax)
                GenerateRandomMap(data);
            else
            {
                StartCoroutine(GenerateMapFeatures(data, wander));
            }
        }

        private IEnumerator GenerateMapFeatures(FloorData data, DrunkenWanderer wanderer)
        {

            DecoratorRules.StartDecorator(ref wanderer.plan, out bool startSet);
            wanderer.plan.Print();
            yield return new WaitForEndOfFrame();
            DecoratorRules.BossDecorator(ref wanderer.plan, out bool bossSet);
            wanderer.plan.Print();
            DecoratorRules.TreasureDecorator(ref wanderer.plan, out bool treasureSet);
            wanderer.plan.Print();
            yield return new WaitForFixedUpdate();

            if (!bossSet | !startSet | !treasureSet)
            {
                UnityEngine.Debug.Log(">>GameManager: Rebuilding map");
                GenerateRandomMap(data);
            }
            else
            {
                UnityEngine.Debug.Log(">>GameManager: Map successfully built.");
                GameManager.FloorPlan = wanderer.plan;
            }

            Destroy(this.gameObject);

        }
            #endregion

        public static void GenerateFloorMapHandler(FloorData data)
        {
            GameObject obj = new GameObject("--FloorMapConvertor--");
            obj.AddComponent<FloorGenerator>().GenerateRandomMap(data);

        }
    }
}