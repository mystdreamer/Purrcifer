using Purrcifer.Data.Defaults;
using System.Collections;
using UnityEngine;


public class Boss_Worm : MonoBehaviour
{
    [System.Serializable]
    public struct RadialDetection
    {
        public float raidus;
    }

    [System.Serializable]
    public class RadialAttack
    {
        public RadialDetection attackInitalRaidus;

        public void PreformAttack()
        {
            //Calculate inital point. 
            //Calculate firing points. 
            //Calculate unit vectors from boss to point. 
            //Generate object. 
            //Set to object. 
        }
    }

    public class DashAttack
    {
        public float offset; 
        public float roomWidth = DefaultRoomData.DEFAULT_WIDTH;
        public float roomHeight = DefaultRoomData.DEFAULT_HEIGHT;
        public float attackTime = 5f; 

        public float GetDivisionAWidth => roomWidth / 3;
        public float HalfBossPosHeight => (roomHeight / 2) + offset;
        public int GetRandomZone => UnityEngine.Random.Range(1, 4);

        public IEnumerator PreformAttack(GameObject target, Vector3 roomCenter, ref bool attackComplete)
        {
            float width = GetDivisionAWidth;
            float height = roomHeight;
            float step = roomHeight / attackTime;
            float currentTime = 0; 

            Vector3 attackCenter = CalculateAttackCenter(roomCenter);
            Vector3 bossInitalPoint = Vector3.zero;
            Vector3 bossEndPoint = Vector3.zero;
            CalculateBossPoints(attackCenter, ref bossInitalPoint, ref bossEndPoint);

            while (currentTime < attackTime)
            {
                currentTime += Time.deltaTime; 
                target.transform.position = bossInitalPoint + Vector3.down * step;
                yield return null;
            }
        }

        private Vector3 CalculateAttackCenter(Vector3 roomCenter)
        {
            float zone = GetRandomZone;
            Vector3 attackCenter = roomCenter;
            switch (zone)
            {
                case 1:
                    attackCenter.x -= roomWidth / 2; //Move point to min point.
                    break;
                case 2:
                    break;
                case 3:
                    attackCenter.x += roomWidth / 2; //Move point to min point.
                    break;
            }

            return attackCenter;
        }

        private void CalculateBossPoints(Vector3 attackCenter, ref Vector3 initalPoint, ref Vector3 endPoint)
        {
            initalPoint = endPoint = attackCenter;
            initalPoint.z -= HalfBossPosHeight;
            endPoint.z += HalfBossPosHeight;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
