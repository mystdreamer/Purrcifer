using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public FloorPlan plan;
    public GameObject playerPrefab;
    public GameObject playerCameraPrefab; 

    public static GameManager Instance => _instance;
    
    
    void Awake()
    {
        #region Singleton Setup. 
        if (_instance == null)
        {
            _instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        #endregion

    }

    void Update()
    {
        
    }

    public void SpawnCamera(Vector3 position)
    {
        GameObject camera = GameObject.Instantiate(playerCameraPrefab);
        camera.transform.position = position;
    }

    public void SpawnPlayerInstance(Vector3 position)
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
        player.transform.position = position;
    }

    public void GenerateRandomMap(FloorData data)
    {
        StartCoroutine(GenerateMap(data));
    }

    private IEnumerator GenerateMap(FloorData data)
    {
        bool success = false;
        FloorPlan floorPlan = null;

        while (!success)
        {
            floorPlan = MapGenerator(data, out success);
            if (!success)
            {
                Debug.Log(">>GameManager: Rebuilding map");
                yield return new WaitForEndOfFrame();
            }
            else
                Debug.Log(">>GameManager: Map successfully built.");
        }

        plan = floorPlan;
    }

    private static FloorPlan MapGenerator(FloorData data, out bool result)
    {
        DecoratorRule startDecorator = new StartDecorator();
        DecoratorRule bossDecorator = new ExitDecorator();
        DecoratorRule treasureDecorator = new TreasureDecorator();
        FloorPlan plan = DrunkenWanderer.GenerateFloorMap(data);
        bool startSet;
        bool bossSet;
        bool treasureSet;

        startDecorator.Decorate(plan, out startSet);
        bossDecorator.Decorate(plan, out bossSet);
        treasureDecorator.Decorate(plan, out treasureSet);
        FloorPlan.Print2DArray<int>(plan.plan);

        result = (bossSet == true && startSet == true && treasureSet == true);
        return plan;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int x = 0; x < plan.plan.GetLength(0); x++)
            {
                for (int y = 0; y < plan.plan.GetLength(1); y++)
                {
                    if (plan.plan[x, y] == 1)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
                    }

                    if (plan.plan[x, y] == 2)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
                    }

                    if (plan.plan[x, y] == 3)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
                    }

                    if (plan.plan[x, y] == 4)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(new Vector3Int(x, 0, y), 0.5f);
                    }
                }
            }
        }
    }
}
