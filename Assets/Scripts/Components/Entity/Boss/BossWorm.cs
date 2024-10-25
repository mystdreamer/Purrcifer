using Purrcifer.BossAI;
using Purrcifer.Data.Defaults;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public bool growthActive = false;


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
            if (!hasGrowthOverTime || !growthActive)
                return;

            scaleTime += Time.deltaTime;
            currentScaleIncrement = Vector3.Lerp(initalSize, endSize, scaleTime);
            transform.localScale = currentScaleIncrement;
        }

        private void OnCollisionEnter(Collision collision) => ResolveCollision(gameObject);

        private void OnTriggerEnter(Collider other) => ResolveCollision(gameObject);

        private void ResolveCollision(GameObject collision)
        {
            if (gameObject.tag == "Player")
            {
                GameManager.Instance.PlayerState.Health -= 1;
            }
            if (gameObject.name == "Crystal" | hasDirection)
                GameObject.Destroy(gameObject);
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
            obj.transform.position = new Vector3(position.x, 1.2F, position.z);
            controller.direction = direction;
            controller.speed = speed;
            controller.hasDirection = true;
        }

        public static GameObject GenerateScale(GameObject bulletPrefab, Vector3 position, Vector3 initialSize, Vector3 growthSize, float lifetime, float speed)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab);
            BulletControllerWorm controller = obj.AddComponent<BulletControllerWorm>();
            obj.transform.position = new Vector3(position.x, 0, position.z);
            controller.speed = speed;
            controller.hasGrowthOverTime = true;
            controller.initalSize = initialSize;
            controller.endSize = growthSize - initialSize;
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
            NORMAL = 120,
            CRY = 145,
            CRY_HARDER = 160
        }

        private enum BulletCount : int
        {
            NORMAL = 9,
            CRY = 12,
            CRY_HARDER = 15
        }

        private enum BulletWaves : int
        {
            NORMAL = 6,
            CRY = 10,
            CRY_HARDER = 20
        }

        private const float BLOCKER_OFFSET = 6;
        private const float BULLET_SPAWN_RADIUS = 4F;
        private const float BULLET_LIFESPAN = 3.5F;
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
                SpawnWave(adjustedCentre, 0, speed, count);
                yield return new WaitForSeconds(0.35f);
            }

            yield return new WaitForSeconds(0.25F);

            Destroy(a);
            Destroy(b);

            complete = true;
        }

        public void SpawnWave(Vector3 centre, float offset, float speed, int count)
        {
            //Calculate firing points. 
            Vector3[] positions = CalculateRadialPoints(centre, offset, count);

            //Calculate unit vectors from boss to point.
            Vector3[] directions = CalculateDirectionVecs(positions, centre);

            //Generate objects. 
            for (int i = 0; i < positions.Length; i++)
            {
                BulletControllerWorm.Generate(bulletTypePrefab, centre + (positions[i] * BULLET_SPAWN_RADIUS) + new Vector3(0, 1.5F, 0), directions[i], (int)speed, BULLET_LIFESPAN);
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

        public Vector3[] CalculateRadialPoints(Vector3 position, float offset, float numberToSpawn)
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

        public Vector3[] CalculateDirectionVecs(Vector3[] bulletPoints, Vector3 initalPoint)
        {
            Vector3[] vecs = new Vector3[bulletPoints.Length];

            for (int i = 0; i < bulletPoints.Length; i++)
            {
                vecs[i] = (initalPoint - initalPoint + bulletPoints[i]).normalized;
            }

            return vecs;
        }
    }

    [System.Serializable]
    public class DashAttack
    {
        private Vector3 _telegraphOffset = new Vector3(0, 0, 18);
        private float _roomWidth = DefaultRoomData.DEFAULT_WIDTH;
        private float _roomHeight = DefaultRoomData.DEFAULT_HEIGHT;
        private float _attackTime = 2f;
        public float offset;
        public float speed = 1f;
        public GameObject downDashObjectPrefab;
        public GameObject dashTelegraph;
        public bool attackStarted = false;
        public bool attackComplete = false;

        public float GetDivisionAWidth => _roomWidth / 3;
        public float HalfBossPosHeight => (_roomHeight / 2) + offset;
        public int GetRandomZone => UnityEngine.Random.Range(1, 4);
        private float BossMovementStep => _roomHeight / _attackTime;

        public IEnumerator PreformAttack(Vector3 roomCenter)
        {
            attackStarted = true;
            float width = GetDivisionAWidth;
            float height = _roomHeight;
            float step = _roomHeight / _attackTime;
            float currentTime = 0;

            Vector3 attackCenter = CalculateAttackCenter(roomCenter);
            Vector3 bossIPoint = Vector3.zero;
            Vector3 bossEPoint = Vector3.zero;
            CalculateBossPoints(attackCenter, ref bossIPoint, ref bossEPoint);
            bossIPoint.y = 1;

            GameObject currentDuplicate = GameObject.Instantiate(downDashObjectPrefab);
            GameObject telegraph = GameObject.Instantiate(dashTelegraph);
            currentDuplicate.transform.position = bossIPoint;
            telegraph.transform.position = bossIPoint + new Vector3(0, 0, 18);

            //Show telegraph.
            telegraph.SetActive(true);
            yield return new WaitForSeconds(0.55F);
            Destroy(telegraph);

            //Move boss item. 
            currentDuplicate.SetActive(true);

            while (currentTime < _attackTime)
            {
                currentTime += Time.deltaTime;
                currentDuplicate.transform.position += Vector3.back * speed;
                yield return null;
            }

            Destroy(currentDuplicate);
            attackComplete = true;
        }

        private Vector3 CalculateAttackCenter(Vector3 roomCenter)
        {
            float zone = GetRandomZone;
            Vector3 attackCenter = roomCenter;
            switch (zone)
            {
                case 1:
                    attackCenter.x -= (_roomWidth / 2) - 8; //Move point to min point.
                    break;
                case 2:
                    break;
                case 3:
                    attackCenter.x += (_roomWidth / 2) - 5; //Move point to min point.
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

            float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;

            Vector3 spawnPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(6, 12);

            //Display telegraph here. 
            yield return new WaitForSeconds(0.45F);

            //Spawn bullet. 
            GameObject spawn = BulletControllerWorm.GenerateScale(prefabToSpawn, initialPoint + spawnPos, Vector3.one, new Vector3(30, 0, 30), 0.5F, 0.002F);

            yield return new WaitForSeconds(0.26F);
            spawn.GetComponent<BulletControllerWorm>().growthActive = true;

            yield return new WaitForSeconds(1.6F);
            Destroy(spawn);
            attackComplete = true;
        }
    }

    public enum BossState
    {
        NONE = -1,
        AWAKE = 0,
        IDLE = 1,
        DASH = 2,
        SPAWN = 3,
        BLOCK_ATTACK = 4,
        INACTIVE = 100,
    }

    private const float SHORT_WAIT = 0.85F;
    private const float HIDE_BOSS_DELAY = 1.5f;
    private const float INTER_UPDATE_DELAY = 0.01f;
    private GameObject bossSpawn;
    private bool bossShowing = false;
    private float emergenceDistance = 6F;
    public BlockingAttack blockingAttack;
    public DashAttack dashAttack;
    public SpawnAttack spawnAttack;
    public BossState bossState = BossState.INACTIVE;
    private int[] randArr = { 1, 2, 2, 2, 3, 3, 4 };
    float idleDuration = 0.95F;
    public GameObject standingPrefab;

    private void Update() => StateManager();

    private void StateManager()
    {
        switch (bossState)
        {
            case BossState.AWAKE:
                bossState = BossState.NONE;
                StartCoroutine(IdleTimer());
                break;
            case BossState.IDLE:
                bossState = BossState.NONE;
                StartCoroutine(IdleTimer());
                break;
            case BossState.DASH:
                bossState = BossState.NONE;
                StartCoroutine(State_Dash());
                break;
            case BossState.SPAWN:
                bossState = BossState.NONE;
                StartCoroutine(State_Spawn());
                break;
            case BossState.BLOCK_ATTACK:
                bossState = BossState.NONE;
                StartCoroutine(State_BlockAttack());
                break;
            case BossState.INACTIVE:
                break;
            case BossState.NONE:
                break;
            default:
                break;
        }
    }

    public void RandAttackTransition()
    {
        int id = randArr[UnityEngine.Random.Range(0, randArr.Length)];
        bossState = (BossState)id;
        StateManager();
    }

    private IEnumerator State_Dash()
    {
        StartCoroutine(dashAttack.PreformAttack(Camera.main.transform.position));

        yield return new WaitForSeconds(0.45F);

        dashAttack.attackStarted = false;
        dashAttack.attackComplete = false;
        bossState = BossState.IDLE;
    }

    private IEnumerator State_BlockAttack()
    {
        bossState = BossState.NONE;
        StartCoroutine(ShowBoss(Camera.main.transform.position));

        while (!bossShowing)
            yield return null;

        if (!blockingAttack.started)
        {
            StartCoroutine(blockingAttack.PreformAttack(Camera.main.transform.position, GameManager.WorldState));
        }

        while (!blockingAttack.complete)
            yield return null;


        StartCoroutine(HideBoss());

        while (bossShowing)
            yield return null;

        blockingAttack.started = false;
        blockingAttack.complete = false;
        Destroy(bossSpawn);
        bossState = BossState.IDLE;
    }

    private IEnumerator State_Spawn()
    {
        bossState = BossState.NONE;
        StartCoroutine(ShowBoss(Camera.main.transform.position));

        while (!bossShowing)
            yield return null;

        yield return new WaitForSeconds(SHORT_WAIT);

        StartCoroutine(spawnAttack.PreformAttack(Camera.main.transform.position));

        while (!spawnAttack.attackComplete)
            yield return null;

        yield return new WaitForSeconds(SHORT_WAIT);
        StartCoroutine(HideBoss());

        while (bossShowing)
            yield return null;

        spawnAttack.attacking = spawnAttack.attackComplete = false;
        Destroy(bossSpawn);
        bossState = BossState.IDLE;
    }

    private IEnumerator ShowBoss(Vector3 position)
    {
        bossSpawn = GameObject.Instantiate(standingPrefab);
        bossSpawn.SetActive(true);
        bossSpawn.transform.position = Camera.main.transform.position - new Vector3(0, 12, 0);
        BHealth.Invincible = false;

        float moved = 0;

        while (moved < emergenceDistance)
        {
            moved += emergenceDistance * Time.deltaTime;
            bossSpawn.transform.position += new Vector3(0, emergenceDistance * Time.deltaTime, 0);
            yield return new WaitForSeconds(INTER_UPDATE_DELAY);
        }

        bossShowing = true;
    }

    private IEnumerator HideBoss()
    {
        float moved = emergenceDistance;

        yield return new WaitForSeconds(HIDE_BOSS_DELAY);

        while (moved > 0)
        {
            moved -= emergenceDistance * Time.deltaTime;
            bossSpawn.transform.position -= new Vector3(0, emergenceDistance * Time.deltaTime, 0);
            yield return new WaitForSeconds(INTER_UPDATE_DELAY);
        }

        BHealth.Invincible = true;
        bossShowing = false;
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
        Debug.Log("Boss Dead!");
        StopAllCoroutines();
        Destroy(bossSpawn);
        UIManager.DisableBossHealthBar();
        if (GameManager.Instance.PlayerState.Alive)
        {
            DataManager.DataCarrier.SetEventState(1001, true);
            DataManager.DataCarrier.PlayerEventData.SaveEvents();
        }
        gameObject.SetActive(false);
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
