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

            if (currentLifetime >= lifetime)
                gameObject.SetActive(false);

            body.linearVelocity = direction * (speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerState.Health -= 1;
            }
            Destroy(gameObject);
        }

        public static void Generate(GameObject bulletPrefab, Vector3 position, Vector3 direction, float speed, float lifetime)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab);
            BulletControllerWorm controller = obj.AddComponent<BulletControllerWorm>();
            obj.transform.position = position;
            controller.direction = direction;
            controller.speed = speed;
        }
    }

    [System.Serializable]
    public class BlockingAttack
    {
        private enum BulletSpeeds : int
        {
            NORMAL = 100,
            CRY = 110,
            CRY_HARDER = 130
        }

        private enum BulletCount : int
        {
            NORMAL = 6,
            CRY = 7,
            CRY_HARDER = 8
        }

        private const float BLOCKER_OFFSET = 6;

        public float bulletSpawnRadius;
        public int bulletSpawnCount;
        public float bulletLifetime;
        public GameObject bulletTypePrefab;
        public GameObject bulletBlockerPrefab;
        public bool started = false;
        public bool complete = false;

        private int GetBulletSpeed(WorldState state)
        {
            switch (state)
            {
                case WorldState.WORLD_START:
                    return (int)BulletSpeeds.NORMAL;
                case WorldState.WORLD_WITCHING:
                    return (int)BulletSpeeds.CRY;
                case WorldState.WORLD_HELL:
                    return (int)BulletSpeeds.CRY_HARDER;
                default:
                    return (int)BulletSpeeds.NORMAL;
            }

        }

        private int GetBulletCount(WorldState state)
        {
            switch (state)
            {
                case WorldState.WORLD_START:
                    return (int)BulletCount.NORMAL;
                case WorldState.WORLD_WITCHING:
                    return (int)BulletCount.CRY;
                case WorldState.WORLD_HELL:
                    return (int)BulletCount.CRY_HARDER;
                default:
                    return (int)BulletCount.NORMAL;
            }
        }

        public IEnumerator PreformAttack(Vector3 position, WorldState state)
        {
            started = true;

            //Calculate the center and get the bullet data. 
            Vector3 adjustedCentre = new Vector3(position.x, 1, position.z);
            int speed = GetBulletSpeed(state);
            int count = GetBulletCount(state);

            //Generate the bullet blockers. 
            GenerateBlockingObjects(adjustedCentre, out GameObject a, out GameObject b);

            yield return new WaitForSeconds(0.5F);

            int waveCount = 1;

            switch (state)
            {
                case WorldState.WORLD_WITCHING:
                    waveCount = 4;
                    break;
                case WorldState.WORLD_HELL:
                    waveCount = 8;
                    break;
            }

            for (int i = 0; i < waveCount; i++)
            {
                SpawnWave(adjustedCentre, 25 * i, speed, count);
                yield return new WaitForSeconds(0.55f);
            }

            yield return new WaitForSeconds(0.5F);

            Destroy(a);
            Destroy(b);

            complete = true;
        }

        public void SpawnWave(Vector3 centre, float offset, float speed, int count)
        {
            //Calculate firing points. 
            Vector3[] positions = CalculateRadialPoints(centre, offset, bulletSpawnRadius, count);

            //Calculate unit vectors from boss to point.
            Vector3[] directions = CalculateDirectionVecs(positions, centre, bulletSpawnRadius);

            //Generate objects. 
            for (int i = 0; i < positions.Length; i++)
            {
                BulletControllerWorm.Generate(bulletTypePrefab, centre + positions[i] + new Vector3(0, 1, 0), directions[i], (int)speed, bulletLifetime);
            }
        }

        private void GenerateBlockingObjects(Vector3 centre, out GameObject aBlocker, out GameObject bBlocker)
        {
            Vector3 randDir = Helper_BossAI.RandVectorOneToZero();
            int rand = UnityEngine.Random.Range(0, 2);
            Vector3 a;
            Vector3 b;

            if (rand == 1)
            {
                a = new Vector3(-1, 0, 0);
                b = new Vector3(1, 0, 0);
            }
            else
            {
                a = new Vector3(0, 0, -1);
                b = new Vector3(0, 0, 1);
            }

            aBlocker = GameObject.Instantiate(bulletBlockerPrefab);
            bBlocker = GameObject.Instantiate(bulletBlockerPrefab);
            bBlocker.transform.position = centre + (b * BLOCKER_OFFSET);
            aBlocker.transform.position = centre + (a * BLOCKER_OFFSET);
        }

        public Vector3[] CalculateRadialPoints(Vector3 position, float offset, float radius, float numberToSpawn)
        {
            List<Vector3> points = new List<Vector3>();
            float step = 360F / numberToSpawn;
            Vector3 temp;

            for (int i = 0; i < numberToSpawn; i++)
            {
                float newOffset = offset + step * i;
                temp = new Vector3(Mathf.Cos(newOffset * Mathf.Deg2Rad), 0, Mathf.Sin(newOffset * Mathf.Deg2Rad));
                points.Add(temp);
            }

            return points.ToArray();
        }

        public Vector3[] CalculateDirectionVecs(Vector3[] bulletPoints, Vector3 initalPoint, float radiusFromBoss)
        {
            Vector3[] vecs = new Vector3[bulletPoints.Length];

            for (int i = 0; i < bulletPoints.Length; i++)
            {
                vecs[i] = (initalPoint - initalPoint + bulletPoints[i]).normalized * radiusFromBoss;
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
            bossInitalPoint.y = 1;

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
                    attackCenter.x -= (roomWidth / 2) - 8; //Move point to min point.
                    break;
                case 2:
                    break;
                case 3:
                    attackCenter.x += (roomWidth / 2) - 5; //Move point to min point.
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
        if (dashAttack.attackStarted && dashAttack.attackComplete)
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
        if (!blockingAttack.started)
        {
            StartCoroutine(blockingAttack.PreformAttack(Camera.main.transform.position, GameManager.WorldState));
        }

        if(blockingAttack.started && blockingAttack.complete)
        {
            blockingAttack.started = false;
            blockingAttack.complete = false;
            bossState |= BossState.IDLE;
        }
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
