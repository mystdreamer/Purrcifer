using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Types.FloorGeneration
{
    public class FloorGenerationHandler : MonoBehaviour
    {

        private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

        public void GenerateRandomMap(FloorData data)
        {
            StartCoroutine(DrunkenWanderer(data));
        }

        public IEnumerator DrunkenWanderer(FloorData data)
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
                StartCoroutine(GenerateFloorObjects(data, wanderer.plan));
            }

        }

        private IEnumerator GenerateFloorObjects(FloorData data, FloorPlan plan)
        {
            ObjectMap objMap = new ObjectMap(data, plan);
            int posValue;

            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.plan.GetLength(1); j++)
                {
                    posValue = plan.plan[i, j];
                    objMap.GenerateObject(posValue, i, j);
                    yield return new WaitForEndOfFrame();
                }
            }

            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.plan.GetLength(1); j++)
                {
                    objMap.EnableDoors(i, j);
                    yield return new WaitForEndOfFrame();
                }
            }

            GameManager.Instance.GenerationComplete();
        }
    }
}