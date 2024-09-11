using System.Collections;
using UnityEngine;

namespace FloorGeneration
{
    public class FloorMapConvertor : MonoBehaviour
{
        private void StartObjectConversion(FloorData data, FloorPlan plan)
        {
            StartCoroutine(GenerateObjectConversion(data, plan));
        }

        private IEnumerator GenerateObjectConversion(FloorData data, FloorPlan plan)
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

            GameManager.CurrentObjectMap = objMap; 
            Destroy(this.gameObject);
        }

        public static void GenerateFloorMapConvertor(FloorData data, FloorPlan plan)
        {
            GameObject obj = new GameObject("--FloorMapConvertor--"); 
            obj.AddComponent<FloorMapConvertor>().StartObjectConversion(data, plan);

        }
    }
}