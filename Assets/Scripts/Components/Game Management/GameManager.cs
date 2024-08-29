using UnityEngine;
using Assets.Scripts.Types.FloorGeneration;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public ObjectMap map;
    public GameObject playerPrefab;
    public GameObject playerCurrent; 
    public GameObject playerCameraPrefab;
    public FloorGenerationHandler floorGenerationHandler;
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

    public void SetCamera(Vector3 position)
    {
        Camera.main.transform.position = position;
    }

    public void SpawnPlayerInstance(Vector3 position)
    {
        playerCurrent = GameObject.Instantiate(playerPrefab);
        playerCurrent.transform.position = position;
    }

    public void GenerateRandomMap(FloorData data) => floorGenerationHandler.GenerateRandomMap(data);
}
