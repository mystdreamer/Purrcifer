using UnityEngine;
using FloorGeneration;
using System.Collections;
using Unity.VisualScripting;
using Purrcifer.FloorGeneration;
using Purrcifer.Data.Defaults;
using Purrcifer.Window.Management;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The private singleton reference. 
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Cached reference to the ObjectPoolManager.
    /// </summary>
    [SerializeField] private ObjectPoolManager _objectPoolManager;

    #region Floor/Level data. 
    /// <summary>
    /// The current world clock instance. 
    /// </summary>
    [SerializeField] private WorldClock _worldClock;

    [SerializeField] private FloorData floorData;

    [SerializeField] private FloorPlan floorMap;

    [SerializeField] private ObjectMap _objectMap;

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

    public static ObjectMap CurrentObjectMap
    {
        set => _instance._objectMap = value;
    }
    #endregion

    #region Player Management Properties.
    /// <summary>
    /// The player prefab used for instancing. . 
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// The current player instance. 
    /// </summary>
    [SerializeField] private GameObject _playerCurrent;

    /// <summary>
    /// The current state of the player. 
    /// </summary>
    [SerializeField] private PlayerState _playerState;

    /// <summary>
    /// Cached reference to the player movement system. 
    /// </summary>
    [SerializeField] private PlayerMovementSys _playerMovementSys;

    /// <summary>
    /// Returns the current instance of the GameManager. 
    /// </summary>
    public static GameManager Instance => _instance;

    /// <summary>
    /// Returns the current world clock instance. 
    /// </summary>
    public static WorldClock WorldClock => _instance._worldClock;

    /// <summary>
    /// Returns the current PlayerState instance. 
    /// </summary>
    public PlayerState PlayerState => _playerState;
    
    /// <summary>
    /// Returns the current player movement system. 
    /// </summary>
    public PlayerMovementSys PlayerMovementSys => _playerMovementSys; 

    /// <summary>
    /// Returns the current player transform. 
    /// </summary>
    public Transform PlayerTransform => _playerCurrent.transform;

    /// <summary>
    /// Returns the current player instance. 
    /// </summary>
    public GameObject Player => _playerCurrent;

    /// <summary>
    /// Returns true if the player currently exists. 
    /// </summary>
    public bool PlayerExists => (_playerCurrent != null);

    /// <summary>
    /// Set the player movement to active/inactive. 
    /// </summary>
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
        _objectPoolManager = new ObjectPoolManager(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
            GameWindowManagement.ManageWindowFullscreen();

    }

    #region Object Pooling
    /// <summary>
    /// Returns an object from the object pool based on the type provided. 
    /// </summary>
    /// <param name="prefab"> The GameObject type to get. </param>
    public GameObject GetFromPool(GameObject prefab) => _objectPoolManager.GetGameObjectType(prefab);

    /// <summary>
    /// Clear all current ObjectPools. 
    /// </summary>
    public void ClearPools() => _objectPoolManager.ClearPools();

    /// <summary>
    /// Clear a certain ObjectPool based on the provided object type. 
    /// </summary>
    /// <param name="prefab"> The prefab to remove. </param>
    public void ClearPoolByType(GameObject prefab) => _objectPoolManager.ClearPoolByType(prefab);
    #endregion

    public void SetEvent(string eventName, int eventID)
    {
        Debug.Log("Item collected: Applying event data.");

    }

    public void ApplyPowerup(PowerupValue value) => PlayerState.ApplyPowerup(value);

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

    #region Teleporting Rooms. 

    /// <summary>
    /// Function teleports the passed object to a room of the given type. 
    /// </summary>
    /// <param name="teleportObject"> The object to teleport. </param>
    /// <param name="marker"> The room type to teleport to. </param>
    public void Teleport(GameObject teleportObject, MapIntMarkers marker)
    {

        GameObject matchedObj = GameManager.Instance.GetRoomByType(marker, out Vector2Int coordinates);
        if (matchedObj != null)
        {
            //If the provided room to teleport to is null, don't teleport. 
            teleportObject.transform.position = matchedObj.transform.position;
            SetCamera(new Vector3(coordinates.x * DefaultRoomData.DEFAULT_WIDTH, coordinates.y * DefaultRoomData.DEFAULT_HEIGHT));
        }
    }
    #endregion

    #region Room Addressing. 
    /// <summary>
    /// Gets a map room of the given type as a gameobject. 
    /// </summary>
    /// <param name="marker"> The marker to locate. </param>
    /// <returns> Gameobject that is the room or null if not located. </returns>
    /// <exception cref="System.Exception"> Throws an exception if the passed in type is NONE, as this room should not be teleported to. </exception>
    private GameObject GetRoomByType(MapIntMarkers marker, out Vector2Int mapCoords)
    {
        if (marker == MapIntMarkers.NONE)
        {
            Debug.Log("Call made to a room type that identifies an empty room. \n Thus teleport was not executed.");

            throw new System.Exception();
        }

        int mapMarkerConversion = (int)marker;

        //Retrieve the positions of matching rooms. 
        Vector2Int[] matched = floorMap.GetTypeMark(mapMarkerConversion).ToArray();

        //Retrieve the room object.
        GameObject room = _objectMap[matched[0]];

        Debug.Log("GameManager: Get Room By Type >> \n Room Address: [" + matched[0].x + ", " + 
            matched[0].y + "]\n" + "Room Name [" + room.name + "]");
        mapCoords = matched[0];

        return room; 
    }
    #endregion

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
        _playerMovementSys = _playerCurrent.GetComponent<PlayerMovementSys>();
        _playerState = _playerCurrent.GetComponent<PlayerState>();
        _playerState.SetPlayerData();
    }
    #endregion
}
