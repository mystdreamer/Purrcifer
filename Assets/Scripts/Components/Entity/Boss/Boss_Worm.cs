using JetBrains.Annotations;
using NUnit.Framework;
using Purrcifer.BossAI;
using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Worm : MonoBehaviour
{
    public class BulletControllerWorm : MonoBehaviour
    {
        public float lifetime = 10f;
        public Vector3 direction;
        public float speed;
        public float currentLifetime = 0; 
        Rigidbody body; 

        public void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        public void FixedUpdate()
        {
            lifetime += Time.deltaTime; 
            
            if(currentLifetime >= lifetime)
                gameObject.SetActive(false);

            body.linearVelocity = direction * (speed * Time.deltaTime); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerState.Health -= 1; 
                gameObject.SetActive(false);
            }
        }

        public static void Generate(GameObject bulletPrefab, Vector3 size, Vector3 direction, float speed, float lifetime)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab);
            BulletControllerWorm controller = obj.GetComponent<BulletControllerWorm>();
            obj.transform.localScale = size;
            controller.direction = direction;
            controller.speed = speed;
        }
    }

    [System.Serializable]
    public class BlockingAttack
    {
        public float blockerOffset;
        public float bulletSpawnRadius;
        public int bulletSpawnCount;
        public float bulletSpeed;
        public float bulletLifetime; 
        public GameObject bulletTypePrefab;
        public GameObject bulletBlockerPrefab; 

        public void PreformAttack(Vector3 position)
        {
            //cache inital point. 
            Vector3 initalPoint = position;

            //Generate the bullet blockers. 
            GenerateBlockingObjects(initalPoint);

            //Calculate firing points. 
            Vector3[] bulletPositions = CalculateRadialPoints(position, bulletSpawnRadius, bulletSpawnCount);

            //Calculate unit vectors from boss to point.
            Vector3[] directionVecs = CaculateDirectionVecs(bulletPositions, initalPoint, bulletSpawnRadius);
            
            //Generate object. 
            for (int i = 0; i < bulletPositions.Length; i++)
            {
                BulletControllerWorm.Generate(bulletTypePrefab, Vector3.one, directionVecs[i], bulletSpeed, bulletLifetime);
            }
        }

        private void GenerateBlockingObjects(Vector3 centre)
        {
            Vector3 randDir = Helper_BossAI.RandVectorOneToZero();
            Vector3 posBlockerPosition =
                centre + new Vector3(randDir.x * bulletSpawnRadius, 0, randDir.z * bulletSpawnRadius);
            Vector3 negBlockerPosition =
                centre + new Vector3(randDir.x * bulletSpawnRadius, 0, randDir.z * bulletSpawnRadius);
            GameObject.Instantiate(bulletBlockerPrefab).transform.position = posBlockerPosition;
            GameObject.Instantiate(bulletBlockerPrefab).transform.position = negBlockerPosition;
        }

        public Vector3[] CalculateRadialPoints(Vector3 position, float radius, float numberToSpawn)
        {
            List<Vector3> points = new List<Vector3>();
            float step = 360F / numberToSpawn;
            float currentRadius = 0;
            Vector3 temp;

            for (int i = 0; i < numberToSpawn; i++)
            {
                currentRadius += step;
                temp = new Vector3(Mathf.Cos(currentRadius * Mathf.Deg2Rad) * Mathf.Rad2Deg, Mathf.Sin(currentRadius * Mathf.Deg2Rad) * Mathf.Rad2Deg);
                points.Add(temp);
            }

            return points.ToArray();
        }

        public Vector3[] CaculateDirectionVecs(Vector3[] bulletPoints, Vector3 initalPoint, float radiusFromBoss)
        {
            Vector3[] vecs = new Vector3[bulletPoints.Length];

            for (int i = 0;i < bulletPoints.Length; i++)
            {
                vecs[i] = (initalPoint - bulletPoints[i]).normalized * radiusFromBoss; 
            }

            return vecs;
        }
    }

    [System.Serializable]
    public class DashAttack
    {
        public float offset;
        public float roomWidth = DefaultRoomData.DEFAULT_WIDTH;
        public float roomHeight = DefaultRoomData.DEFAULT_HEIGHT;
        public float attackTime = 5f;

        public float GetDivisionAWidth => roomWidth / 3;
        public float HalfBossPosHeight => (roomHeight / 2) + offset;
        public int GetRandomZone => UnityEngine.Random.Range(1, 4);

        public IEnumerator PreformAttack(GameObject target, Vector3 roomCenter)
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

    public class SpawnAttack
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
