using Purrcifer.FloorGeneration.RoomResolution;
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
            FloorMapConversion.GenerateObjectMap(plan, ref objMap);
            FloorMapConversion.OpenDoors(plan, ref objMap);
            GameManager.Instance.SetObjectMap(objMap);
            yield return new WaitForEndOfFrame(); 
            Destroy(this.gameObject);
        }

        public static void GenerateFloorMapConvertor(FloorData data, FloorPlan plan)
        {
            GameObject obj = new GameObject("--FloorMapConvertor--"); 
            obj.AddComponent<FloorMapConvertor>().StartObjectConversion(data, plan);

        }
    }
}