using Purrcifer.BossAI;
using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWorm : Entity
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
            Vector3[] directionVecs = CalculateDirectionVecs(bulletPositions, initalPoint, bulletSpawnRadius);
            
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

        public Vector3[] CalculateDirectionVecs(Vector3[] bulletPoints, Vector3 initalPoint, float radiusFromBoss)
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
        float roomWidth = DefaultRoomData.DEFAULT_WIDTH;
        float roomHeight = DefaultRoomData.DEFAULT_HEIGHT;
        float attackTime = 2f;
        public float speed = 1f; 
        public GameObject downDashObjectPrefab;
        public GameObject dashTelegraph;
        public bool attackStarted = false;         
        public bool attackComplete = false;
        public float telegraphOffset = 0.25F;
        public float GetDivisionAWidth => roomWidth / 3;
        public float HalfBossPosHeight => (roomHeight / 2) + offset;
        public int GetRandomZone => UnityEngine.Random.Range(1, 4);

        public IEnumerator PreformAttack(Vector3 roomCenter)
        {
            attackStarted = true;
            float width = GetDivisionAWidth;
            float height = roomHeight;
            float step = roomHeight / attackTime;
            float currentTime = 0;

            Vector3 attackCenter = CalculateAttackCenter(roomCenter);
            Vector3 bossInitalPoint = Vector3.zero;
            Vector3 bossEndPoint = Vector3.zero;
            CalculateBossPoints(attackCenter, ref bossInitalPoint, ref bossEndPoint);

            GameObject currentDuplicate = GameObject.Instantiate(downDashObjectPrefab); 
            GameObject telegraph = GameObject.Instantiate(dashTelegraph);
            currentDuplicate.transform.position = bossInitalPoint;
            telegraph.transform.position = bossInitalPoint + new Vector3(0, 0, 18);
            
            //Show telegraph.
            telegraph.SetActive(true);
            yield return new WaitForSeconds(0.25F);

            //Move boss item. 
            currentDuplicate.SetActive(true);

            while (currentTime < attackTime)
            {
                currentTime += Time.deltaTime;
                currentDuplicate.transform.position += Vector3.back * speed;
                yield return null;
            }

            Destroy(currentDuplicate);
            Destroy(telegraph);
            attackComplete = true;
        }

        private Vector3 CalculateAttackCenter(Vector3 roomCenter)
        {
            float zone = GetRandomZone;
            Vector3 attackCenter = roomCenter;
            switch (zone)
            {
                case 1:
                    attackCenter.x -= (roomWidth / 2) + 10; //Move point to min point.
                    break;
                case 2:
                    break;
                case 3:
                    attackCenter.x += (roomWidth / 2) - 10; //Move point to min point.
                    break;
            }

            return attackCenter;
        }

        private void CalculateBossPoints(Vector3 attackCenter, ref Vector3 initialPoint, ref Vector3 endPoint)
        {
            initialPoint = endPoint = attackCenter;
            initialPoint.z -= HalfBossPosHeight;
            endPoint.z += HalfBossPosHeight;
        }
    }

    public class SpawnAttack
    {
        
    }

    public class BulletWave
    {

    }

    public enum BossState
    {
        AWAKE = 0, 
        IDLE = 1, 
        DASH = 2, 
        UNDERGROUND = 3, 
        SPAWN = 4, 
        BULLET_WAVE = 5,
        BLOCK_ATTACK = 6,
        INACTIVE = 100,
    }

    public BlockingAttack blockingAttack;
    public DashAttack dashAttack;
    public SpawnAttack spawnAttack;
    public BossState bossState = BossState.INACTIVE;
    public int[] randArr = { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6 };
    public bool idleStarted = false;
    public float idleDuration = 3F; 

    private void Update()
    {
        StateManager();
    }

    private void StateManager()
    {
        switch (bossState)
        {
            case BossState.AWAKE:
                RandAttackTransition();
                break;
            case BossState.IDLE:
                if (!idleStarted)
                    State_Idle();
                break;
            case BossState.DASH:
                State_Dash();
                break;
            case BossState.UNDERGROUND:
                State_Underground();
                break;
            case BossState.SPAWN:
                State_Spawn();
                break;
            case BossState.BULLET_WAVE:
                State_BulletWave();
                break;
            case BossState.BLOCK_ATTACK:
                State_BlockAttack();
                break;
            case BossState.INACTIVE:
                break;
            default:
                break;
        }
    }

    public void RandAttackTransition()
    {
        int id = randArr[UnityEngine.Random.Range(0, randArr.Length - 1)];
        bossState = (BossState)id;
        StateManager();
    }

    private void State_Awake()
    {
        //Add awake animation/effects here. 

        RandAttackTransition();
    }

    private void State_Idle()
    {
        idleStarted = true;
        StartCoroutine(IdleTimer());
    }

    private void State_Dash()
    {
        if (!dashAttack.attackStarted)
        {
            StartCoroutine(dashAttack.PreformAttack(Camera.main.transform.position));
        }
        if(dashAttack.attackStarted && dashAttack.attackComplete)
        {
            dashAttack.attackStarted = false;
            dashAttack.attackComplete = false;
            RandAttackTransition();
        }
    }

    private void State_Underground()
    {
        bossState = BossState.IDLE;
    }

    private void State_BulletWave()
    {
        bossState = BossState.IDLE;
    }

    private void State_BlockAttack()
    {
        bossState = BossState.IDLE;
    }

    private void State_Spawn()
    {
        bossState = BossState.IDLE;
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(idleDuration);
        RandAttackTransition();
    }

    #region Inbuilts.

    internal override void SetWorldState(WorldState state)
    {
    }

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {
    }

    internal override void OnDeathEvent()
    {
    }

    internal override void InvincibilityActivated()
    {
    }

    internal override void OnAwakeObject()
    {
    }

    internal override void OnSleepObject()
    {
    }
    #endregion
}
