using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using Purrcifer.BossAI;
using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWorm : Boss
{
    public class BulletControllerWorm : MonoBehaviour
    {
        Rigidbody body;
        private Vector3 currentScaleIncrement = Vector3.zero;
        public float lifetime = 10f;
        public Vector3 direction;
        public float speed;
        public float currentLifetime = 0;
        public Vector3 initalSize;
        public Vector3 endSize;
        public bool hasGrowthOverTime = false;
        public bool hasDirection = false;
        public bool deactivateOnCollision = true;


        public void Start()
        {
            StartCoroutine(LifeTime(lifetime));
            body = GetComponent<Rigidbody>();
            if (hasGrowthOverTime)
                transform.localScale = initalSize;
        }

        public void FixedUpdate()
        {
            lifetime += Time.deltaTime;

            if (currentLifetime >= lifetime)
                Destroy(gameObject);

            if (hasDirection)
                body.linearVelocity = direction * (speed * Time.deltaTime);

            UpdateGrowth();
        }

        private float scaleTime = 0;

        private void UpdateGrowth()
        {
            if (!hasGrowthOverTime)
                return;

            scaleTime += Time.deltaTime;
            currentScaleIncrement = Vector3.Lerp(initalSize, endSize, scaleTime);
            transform.localScale = currentScaleIncrement;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerState.Health -= 1;
            }
            if (deactivateOnCollision)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerState.Health -= 1;
            }
            if (deactivateOnCollision)
                Destroy(gameObject);
        }

        private IEnumerator LifeTime(float time)
        {
            yield return new WaitForSeconds(time);
            GameObject.Destroy(gameObject);
        }

        public static void Generate(GameObject bulletPrefab, Vector3 position, Vector3 direction, float speed, float lifetime)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab);
            BulletControllerWorm controller = obj.AddComponent<BulletControllerWorm>();
            obj.transform.position = position;
            controller.direction = direction;
            controller.speed = speed;
            controller.hasDirection = true;
        }

        public static GameObject GenerateScale(GameObject bulletPrefab, Vector3 position, Vector3 initalSize, Vector3 growthSize, float lifetime, float speed)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab);
            BulletControllerWorm controller = obj.AddComponent<BulletControllerWorm>();
            obj.transform.position = new Vector3(position.x, 0, position.z);
            controller.speed = speed;
            controller.hasGrowthOverTime = true;
            controller.initalSize = initalSize;
            controller.endSize = growthSize - initalSize;
            controller.endSize.y = controller.initalSize.y = 8F;
            controller.deactivateOnCollision = false;
            obj.GetComponent<SphereCollider>().isTrigger = true;
            return obj;
        }
    }

    [System.Serializable]
    public class BlockingAttack
    {
        private enum BulletSpeeds : int
        {
            NORMAL = 100,
            CRY = 125,
            CRY_HARDER = 150
        }

        private enum BulletCount : int
        {
            NORMAL = 4,
            CRY = 6,
            CRY_HARDER = 8
        }

        private enum BulletWaves : int
        {
            NORMAL = 6,
            CRY = 10,
            CRY_HARDER = 20
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

        private int GetBulletWaveCount(WorldState state)
        {
            switch (state)
            {
                case WorldState.WORLD_START:
                    return (int)BulletWaves.NORMAL;
                case WorldState.WORLD_WITCHING:
                    return (int)BulletWaves.CRY;
                case WorldState.WORLD_HELL:
                    return (int)BulletWaves.CRY_HARDER;
                default:
                    return (int)BulletWaves.NORMAL;
            }
        }

        public IEnumerator PreformAttack(Vector3 position, WorldState state)
        {
            started = true;

            //Calculate the center and get the bullet data. 
            Vector3 adjustedCentre = new Vector3(position.x, 1, position.z);
            int speed = GetBulletSpeed(state);
            int count = GetBulletCount(state);
            int waves = GetBulletWaveCount(state);

            //Generate the bullet blockers. 
            GenerateBlockingObjects(adjustedCentre, out GameObject a, out GameObject b);

            yield return new WaitForSeconds(0.5F);

            for (int i = 0; i < waves; i++)
            {
                SpawnWave(adjustedCentre, 25 * i, speed, count);
                yield return new WaitForSeconds(0.55f);
            }

            yield return new WaitForSeconds(0.25F);

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
            yield return new WaitForSeconds(0.55F);

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

    [System.Serializable]
    public class SpawnAttack
    {
        public GameObject prefabToSpawn;
        public bool attacking = false;
        public bool attackComplete = false;

        public IEnumerator PreformAttack(Vector3 initialPoint)
        {
            attacking = true;
            float roomWidth = DefaultRoomData.DEFAULT_WIDTH - 20;
            float roomHeight = DefaultRoomData.DEFAULT_HEIGHT - 10;

            Vector3 spawnPos = new Vector3(
                UnityEngine.Random.Range(-(roomWidth / 2), (roomWidth / 2)),
                UnityEngine.Random.Range(-(roomWidth / 2), (roomHeight / 2)));

            //Display telegraph here. 

            yield return new WaitForSeconds(0.05F);

            //Spawn bullet. 
            GameObject spawn = BulletControllerWorm.GenerateScale(prefabToSpawn, initialPoint + spawnPos, Vector3.one, new Vector3(30, 0, 30), 1.5F, 0.002F);

            yield return new WaitForSeconds(1.6F);
            Destroy(spawn);
            attackComplete = true;
        }
    }


    public enum BossState
    {
        AWAKE = 0,
        IDLE = 1,
        DASH = 2,
        SPAWN = 4,
        BLOCK_ATTACK = 6,
        INACTIVE = 100,
    }

    public BlockingAttack blockingAttack;
    public DashAttack dashAttack;
    public SpawnAttack spawnAttack;
    public BossState bossState = BossState.INACTIVE;
    public int[] randArr = { 1, 2, 4, 6, 2, 4, 6, 2, 4, 6 };
    public bool idleStarted = false;
    public float idleDuration = 3F;
    public GameObject standingPrefab;

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
            case BossState.SPAWN:
                if (!StateSpawnExecuting)
                    StartCoroutine(State_Spawn());
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

    private void State_BlockAttack()
    {
        if (!blockingAttack.started)
        {
            StartCoroutine(blockingAttack.PreformAttack(Camera.main.transform.position, GameManager.WorldState));
        }

        if (blockingAttack.started && blockingAttack.complete)
        {
            blockingAttack.started = false;
            blockingAttack.complete = false;
            bossState |= BossState.IDLE;
        }
    }

    private bool StateSpawnExecuting = false;

    private IEnumerator State_Spawn()
    {
        StateSpawnExecuting = true;
        GameObject standing = GameObject.Instantiate(standingPrefab);
        standing.SetActive(true);
        standing.transform.position = Camera.main.transform.position - new Vector3(0, 12, 0);

        float moved = 0;

        while (moved < 9)
        {
            moved += 9F * Time.deltaTime;
            standing.transform.position += new Vector3(0, 6F * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(spawnAttack.PreformAttack(Camera.main.transform.position));

        while (!spawnAttack.attackComplete)
        {
            yield return null;
        }

        spawnAttack.attacking = spawnAttack.attackComplete = false;
        RandAttackTransition();
        StateSpawnExecuting = false;
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(idleDuration);
        RandAttackTransition();
    }

    #region Inbuilts.

    internal override void HealthChangedEvent(float lastValue, float currentValue)
    {
    }

    internal override void OnDeathEvent()
    {
    }

    internal override void InvincibilityActivated()
    {
    }

    internal new void Start()
    {
        bossState = BossState.AWAKE;
        UIManager.SetBossHealth(this);
        base.Start();
    }

    internal override void ApplyWorldState(WorldState state)
    {

    }

    internal override void IncomingDamageDisabled()
    {
    }

    internal override void IncomingDamageEnabled()
    {
    }
    #endregion
}
