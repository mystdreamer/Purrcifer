using UnityEngine;
using FloorGeneration;
using System.Collections;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The private singleton reference. 
    /// </summary>
    private static GameManager _instance;

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
    /// The player prefab used for instancing. . 
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// The current player instance. 
    /// </summary>
    public GameObject playerCurrent;

    /// <summary>
    /// The current state of the player. 
    /// </summary>
    public PlayerState playerState;

    /// <summary>
    /// Returns the current instance of the GameManager. 
    /// </summary>
    public static GameManager Instance => _instance;

    /// <summary>
    /// Returns the current world clock instance. 
    /// </summary>
    public static WorldClock WorldClock => _instance._worldClock;

    /// <summary>
    /// Generate a random map based on provided FloorData. 
    /// </summary>
    public static FloorData FloorData
    {
        set
        {
            _instance.floorData = value;
            FloorGeneration.FloorGenerator.GenerateFloorMapHandler(value);
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
        set
        {
            _instance.objectMap = value;
        }
    }

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle fullscreen mode
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void PlayerDeath()
    {
        playerCurrent.GetComponent<MovementSys>().UpdatePause = true;
        UIManager.EnableGameOverScreen();
    }

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
        playerCurrent.GetComponent<MovementSys>().UpdatePause = false;
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

    /// <summary>
    /// Spawn a player instance at the specified location. 
    /// </summary>
    /// <param name="position"> The position to spawn the player at. </param>
    public void SpawnPlayerInstance(Vector3 position)
    {
        playerCurrent = GameObject.Instantiate(playerPrefab);
        playerCurrent.transform.position = position;
        MovementSys mSys = playerCurrent.GetComponent<MovementSys>();
        mSys.UpdatePause = true;
        playerState = playerCurrent.GetComponent<PlayerState>();
        playerState.SetPlayerData();
    }
}
