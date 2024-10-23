using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

namespace Purrcifer.BossAI
{
    public static class Helper_BossAI
    {
        public static Vector3 RandVectorOneToZero()
        {
            return RandomRangeVector(0, 1, 0, 1);
        }

        public static Vector3 RandomRangeVector(
            float xMin, float xMax, float yMin, float yMax
            )
        {
            return new UnityEngine.Vector3(
                UnityEngine.Random.Range(xMin, xMax + 1),
                UnityEngine.Random.Range(yMin, yMax + 1)
                );
        }

        public static List<BossHitbox> GetHitBoxes(GameObject obj)
        {
            List<BossHitbox> hBox = obj.GetComponents<BossHitbox>().ToList();
            hBox.AddRange(obj.GetComponentsInChildren<BossHitbox>());
            return hBox;
        }
    }
}
