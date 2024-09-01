using UnityEngine;
using Assets.Scripts.Types.FloorGeneration;
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
    /// The current floor generation handler instance. 
    /// </summary>
    public FloorGenerationHandler floorGenerationHandler;

    /// <summary>
    /// Returns the current instance of the GameManager. 
    /// </summary>
    public static GameManager Instance => _instance;

    /// <summary>
    /// Returns the current world clock instance. 
    /// </summary>
    public static WorldClock WorldClock => _instance._worldClock;

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
        if(DataCarrier.Instance == null)
        {
            DataCarrier.Generate(); 
        }
#endif
    }

    void Update()
    {

    }

    public void GenerationComplete()
    {
        Debug.Log("Map built.");
        StartCoroutine(FadeWait());
    }

    private IEnumerator FadeWait()
    {
        UIManager.FadeOut();
        while (!UIManager.Instance.FadeOpComplete)
        {
            yield return new WaitForEndOfFrame();
        }

        _worldClock.TimerActive = true;
        playerCurrent.GetComponent<MovementSys>().UpdatePause = false;
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
        playerCurrent.GetComponent<MovementSys>().UpdatePause = true;
        playerState = playerCurrent.GetComponent<PlayerState>();
    }

    /// <summary>
    /// Generates a random map. 
    /// </summary>
    /// <param name="data"></param>
    public void GenerateRandomMap(FloorData data) => floorGenerationHandler.GenerateRandomMap(data);
}
