#define TOP_LEVEL_DEBUG
#undef TOP_LEVEL_DEBUG
#define DEV_SHORTCUTS
//#undef DEV_SHORTCUTS

using UnityEngine;
using FloorGeneration;
using System.Collections;
using Purrcifer.FloorGeneration;
using Purrcifer.Data.Defaults;
using Purrcifer.PlayerData;
using DataManager;
using Purrcifer.Data.Player;

public partial class GameManager : MonoBehaviour
{
    #region Singleton. 
    /// <summary>
    /// The private singleton reference. 
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Returns the current instance of the GameManager. 
    /// </summary>
    public static GameManager Instance => _instance;
    #endregion

    void OnEnable()
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

    private void OnLevelWasLoaded(int level)
    {
        WorldClock.ResetPlayTime();
    }

    private void Update()
    {
#if DEV_SHORTCUTS

        if (Input.GetKey(KeyCode.F1))
        {
            TeleportPlayer(MapIntMarkers.TREASURE);
        }

        if (Input.GetKey(KeyCode.F2))
        {
            TeleportPlayer(MapIntMarkers.BOSS);
        }

        if (Input.GetKey(KeyCode.F3))
        {
            TeleportPlayer(MapIntMarkers.HIDDEN_ROOM);
        }
#endif
    }

    #region Teleporting Rooms. 

    public void TeleportPlayer(MapIntMarkers type)
    {
        Teleport(Player, type, playerPrefab.transform.position.y);
        Teleport(Camera.main.gameObject, type, Camera.main.transform.position.y);
    }

    /// <summary>
    /// Function teleports the passed object to a room of the given type. 
    /// </summary>
    /// <param name="teleportObject"> The object to teleport. </param>
    /// <param name="marker"> The room type to teleport to. </param>
    public void Teleport(GameObject teleportObject, MapIntMarkers marker, float yOverride)
    {
        //Get the room object. 
        GameObject matchedObj = GameManager.Instance.GetRoomByType(marker, out Vector2Int coordinates);
        Vector3 newPos = new Vector3(coordinates.x * DefaultRoomData.DEFAULT_WIDTH, yOverride, coordinates.y * DefaultRoomData.DEFAULT_HEIGHT);
        //If the provided room to teleport to is null, don't teleport. 
        if (matchedObj != null)
        {
            //Else update the objects position and set the cameras new position in the world. 
            teleportObject.transform.position = new Vector3(matchedObj.transform.position.x, yOverride, matchedObj.transform.position.z);
        }
    }
    #endregion

    #region Room Addressing. 
    /// <summary>
    /// Gets a map room of the given type as a GameObject. 
    /// </summary>
    /// <param name="marker"> The marker to locate. </param>
    /// <returns> GameObject that is the room or null if not located. </returns>
    /// <exception cref="System.Exception"> Throws an exception if the passed in type is NONE, as this room should not be teleported to. </exception>
    private GameObject GetRoomByType(MapIntMarkers marker, out Vector2Int mapCoords)
    {
        //If the marker provided is NONE then throw a system errorm because that should not occur. 
        if (marker == MapIntMarkers.NONE)
        {
#if TOP_LEVEL_DEBUG
            Debug.Log("Call made to a room type that identifies an empty room. \n Thus teleport was not executed.");
#endif
            throw new System.Exception();
        }

        int mapMarkerConversion = (int)marker;

        //Retrieve the positions of matching rooms. 
        Vector2Int[] matched = Helpers_FloorPlan.GetMarksWithType(floorMap, mapMarkerConversion);

        //Retrieve the room object.
        GameObject room = _objectMap[matched[0]];

#if TOP_LEVEL_DEBUG
        Debug.Log("GameManager: Get Room By Type >> \n Room Address: [" + matched[0].x + ", " +
            matched[0].y + "]\n" + "Room Name [" + room.name + "]");
#endif
        mapCoords = matched[0];

        return room;
    }
    #endregion

    public static void LoadLevel(Purrcifer.LevelLoading.LevelID lvlToLoad, bool fadeOnLoad = true)
    {
        Debug.Log("World state Reset");
        UIManager.Instance.StartLevelTransitionFade(lvlToLoad, fadeOnLoad);
    }

    public static void DisableLevelTransition()
    {
        UIManager.Instance.FadeLevelTransitionOut();
    }
}

#region Object Pooling. 
public partial class GameManager : MonoBehaviour
{
    /// <summary>
    /// Cached reference to the ObjectPoolManager.
    /// </summary>
    [SerializeField] private ObjectPoolManager _objectPoolManager;

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
}
#endregion

#region Player Management.
public partial class GameManager : MonoBehaviour
{
    #region Player Management Properties.
    /// <summary>
    /// The player prefab used for instancing. 
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
    public static bool PlayerMovementPaused
    {
        set
        {
            if (Instance._playerCurrent != null)
                PlayerMovementSys.UpdatePause = value;
        }
    }

    public static PlayerInputs PlayerInputs => DataCarrier.PlayerInputs;

    public PlayerHealthData GetPlayerHealthData => (PlayerHealthData)DataCarrier.RuntimeData;

    public PlayerDamageData GetPlayerDamageData => (PlayerDamageData)DataCarrier.RuntimeData;

    public PlayerItemData GetPlayerItemData => new PlayerItemData()
    {
        utilityCharges = DataCarrier.RuntimeData.utilityCharges,
        talismanCharges = DataCarrier.RuntimeData.talismanCount
    };
    #endregion

    #region Powerup/Consumable Accessors.

    /// <summary>
    /// Function for applying stat changes to the player. 
    /// </summary>
    /// <param name="value"> The stat changes to apply. </param>
    public Powerup ApplyPowerup
    {
        set => PlayerState.SetPowerup = value;
    }

    public StatUpgradeDataSO ApplyStatUpgrade
    {
        set => PlayerState.ApplyStatUpgrade = value;
    }

    public UtilityDataSO ApplyUtilityUpgrade
    {
        set => PlayerState.ApplyUtilityUpgrade = value;
    }

    public WeaponDataSO ApplyWeaponUpgrade
    {
        set => PlayerState.ApplyWeaponUpgrade = value;
    }

    #endregion

    /// <summary>
    /// Function used to apply event change data to the players current event data. 
    /// </summary>
    /// <param name="eventName"> The event name associated with the event. </param>
    /// <param name="eventID"> The event id associated with the event. </param>
    public void SetPlayerDataEvent(PlayerEventData data)
    {
        Debug.Log("Item collected: Applying event data.");
        //TODO: Implement this. 
    }

    /// <summary>
    /// Set the current camera's position. 
    /// </summary>
    /// <param name="position"> The position to set. </param>
    public void SetCamera(Vector3 position)
    {
        Camera.main.GetComponent<CameraController>().Position = position;
    }

    /// <summary>
    /// Notify the GameManager of the players death. 
    /// </summary>
    public void PlayerDeath()
    {
        PlayerMovementPaused = true;
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

        //Set this to prevent movement prior to map completion and data being set. 
        PlayerMovementPaused = true;
        _playerMovementSys = _playerCurrent.GetComponent<PlayerMovementSys>();
        _playerState = _playerCurrent.GetComponent<PlayerState>();
        _playerState.SetPlayerData(DataCarrier.RuntimeData);
    }

    ///////////////////////////
    /// PLAYER DATA MANAGEMENT. 
    ///////////////////////////

    public static int GetSavedLevel => DataCarrier.SavedLevel;

    public static void ResetPlayerData() => DataCarrier.Instance.ResetPlayerData();

    public static void SetPlayerData(PlayerStartingStatsSO data) => DataCarrier.Instance.SetPlayerData(data);
}
#endregion

#region World state management. 
public partial class GameManager : MonoBehaviour
{
    /// <summary>
    /// Returns the current world clock instance. 
    /// </summary>
    public static WorldClock WorldClock => _instance._worldClock;

    #region Floor/Level data. 
    /// <summary>
    /// The current world clock instance. 
    /// </summary>
    [SerializeField] private WorldClock _worldClock;

    /// <summary>
    /// The current floor data. 
    /// </summary>
    [SerializeField] private FloorData floorData;

    /// <summary>
    /// The current generated floor plan.  
    /// </summary>
    [SerializeField] private FloorPlan floorMap;

    /// <summary>
    /// The current object map. 
    /// </summary>
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
            FloorGenerationHandler handler = Instance.gameObject.AddComponent<FloorGenerationHandler>();
            handler.GenerateBaseMap(value);
        }
    }

    /// <summary>
    /// Generate a random map based on provided FloorData. 
    /// </summary>
    public FloorPlan FloorPlan
    {
        set
        {
            _instance.floorMap = value;
            FloorMapConvertor.GenerateFloorMapConvertor(_instance.floorData, _instance.floorMap);
        }
    }

    /// <summary>
    /// Accessor for the games current world time. 
    /// </summary>
    public static float WorldTime
    {
        get => Instance._worldClock.PlayTime;
        set => Instance._worldClock.PlayTime = value;
    }

    public static WorldState WorldState => Instance._worldClock.CurrentState;
    #endregion

    #region Delegates.

    //-------------------------------------------
    // These delegates are used for pushing changes in world state to all entities, 
    // Room Objects and bosses.
    // These and any objects that require Realtime updating should subscribe to this rather than doing per frame calls.
    //-------------------------------------------

    /// <summary>
    /// Delegate for world state changes. 
    /// </summary>
    /// <param name="state"> The state to apply. </param>
    public delegate void WorldUpdateEvent(WorldState state);

    /// <summary>
    /// Event for notifying that world state changes have occurred. 
    /// Subscribe to event for world state notifications. 
    /// </summary>
    public WorldUpdateEvent WorldStateChange;

    #endregion

    /// <summary>
    /// Set the object map to the GameManager.
    /// </summary>
    /// <param name="value"></param>
    public void SetObjectMap(ObjectMap value)
    {
        _objectMap = value;
        StartCoroutine(RemoveLoadingScreen());

#if TOP_LEVEL_DEBUG
        Debug.Log("Map built.");
#endif
    }

    /// <summary>
    /// IEnumerator used to handle the transition out of the loading screen. 
    /// </summary>
    /// <returns></returns>
    // In GameManager.cs, modify RemoveLoadingScreen:
    private IEnumerator RemoveLoadingScreen()
    {
        // Call transition out. 
        UIManager.Instance.FadeLevelTransitionOut();
        yield return new WaitForSeconds(0.5f);

        // Wait for UI transition to be complete. 
        while (!UIManager.Instance.TransitionInactive)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Add this before enabling world clock
        if (SoundManager.Instance != null)
        {
            Debug.Log("Notifying SoundManager of level load");
            SoundManager.Instance.OnLevelLoaded();
        }
        else
        {
            Debug.LogError("SoundManager not found when loading level");
        }

        // Enable the world clock.
        _worldClock.ResetPlayTime();
        _worldClock.TimerActive = true;

        // Enable player movement. 
        PlayerMovementPaused = false;
        yield return true;
    }
}
#endregion