using UnityEngine;
using FloorGeneration;
using System.Collections;
using Unity.VisualScripting;
using Purrcifer.FloorGeneration;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The private singleton reference. 
    /// </summary>
    private static GameManager _instance;

    [SerializeField] private ObjectPoolManager objectManager;

    #region Floor/Level data. 
    /// <summary>
    /// The current world clock instance. 
    /// </summary>
    [SerializeField] private WorldClock _worldClock;

    [SerializeField] private FloorData floorData;

    [SerializeField] private FloorPlan floorMap;

    [SerializeField] private ObjectMap objectMap;

    /// <summary>
    /// The current object map.
    /// </summary>
    public ObjectMap map;

    /// <summary>
    /// Generate a random map based on provided FloorData. 
    /// </summary>
    public static FloorData FloorData
    {
        set
        {
            _instance.floorData = value;
            //FloorGeneration.FloorGenerator.GenerateFloorMapHandler(value);
            FloorGenerationHandler handler = Instance.gameObject.AddComponent<FloorGenerationHandler>();
            handler.GenerateBaseMap(value);
        }
    }

    /// <summary>
    /// Generate a random map based on provided FloorData. 
    /// </summary>
    public static FloorPlan FloorPlan
    {
        set
        {
            _instance.floorMap = value;
            FloorMapConvertor.GenerateFloorMapConvertor(_instance.floorData, _instance.floorMap);
            Debug.Log("Map built.");
            _instance.GenerationComplete();
        }
    }

    public static ObjectMap ObjectMap
    {
        set => _instance.objectMap = value;
    }
    #endregion

    #region Player Management.
    /// <summary>
    /// The player prefab used for instancing. . 
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// The current player instance. 
    /// </summary>
    private GameObject _playerCurrent;

    /// <summary>
    /// The current state of the player. 
    /// </summary>
    [SerializeField] private PlayerState _playerState;

    [SerializeField] private PlayerMovementSys _movementSys;

    /// <summary>
    /// Returns the current instance of the GameManager. 
    /// </summary>
    public static GameManager Instance => _instance;

    /// <summary>
    /// Returns the current world clock instance. 
    /// </summary>
    public static WorldClock WorldClock => _instance._worldClock;

    public PlayerState PlayerState => _playerState;
    
    public PlayerMovementSys PlayerMovementSys => _movementSys; 

    public Transform PlayerTransform => _playerCurrent.transform;

    public GameObject Player => _playerCurrent;

    public bool PlayerExists => (_playerCurrent != null);

    public static bool MovementPaused
    {
        set {
            if (Instance._playerCurrent != null)
                Instance._playerCurrent.GetComponent<PlayerMovementSys>().UpdatePause = value;
        }  
    }
    #endregion

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

#if UNITY_EDITOR_WIN
        //If in the editor, check if the data carrier has been loaded.
        if (DataCarrier.Instance == null)
        {
            DataCarrier.Generate();
        }
#endif

        //Generate ObjectPoolManager. 
        objectManager = new ObjectPoolManager(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle fullscreen mode
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    #region Object Pooling
    public GameObject GetFromPool(GameObject prefab) => objectManager.GetGameObjectType(prefab);

    public void ClearPools() => objectManager.ClearPools();

    public void ClearPoolByType(GameObject prefab) => objectManager.ClearPoolByType(prefab);
    #endregion

    public void GenerationComplete()
    {
        StartCoroutine(FadeWait());
    }

    private IEnumerator FadeWait()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.FadeLevelTransitionOut();
        while (!UIManager.Instance.TransitionInactive)
        {
            yield return new WaitForSeconds(0.5f);
        }

        _worldClock.TimerActive = true;
        MovementPaused = false;
        yield return true;
    }

    /// <summary>
    /// Set the current camera's position. 
    /// </summary>
    /// <param name="position"> The position to set. </param>
    public void SetCamera(Vector3 position)
    {
        Camera.main.GetComponent<CameraController>().Position = position;
    }

    #region Player Management.
    /// <summary>
    /// Notify the GameManager of the players death. 
    /// </summary>
    public void PlayerDeath()
    {
        MovementPaused = true;
        UIManager.EnableGameOverScreen();
    }

    /// <summary>
    /// Spawn a player instance at the specified location. 
    /// </summary>
    /// <param name="position"> The position to spawn the player at. </param>
    public void SpawnPlayerInstance(Vector3 position)
    {
        _playerCurrent = GameObject.Instantiate(playerPrefab);
        _playerCurrent.transform.position = position;
        MovementPaused = true;
        _movementSys = _playerCurrent.GetComponent<PlayerMovementSys>();
        _playerState = _playerCurrent.GetComponent<PlayerState>();
        _playerState.SetPlayerData();
    }
    #endregion
}
