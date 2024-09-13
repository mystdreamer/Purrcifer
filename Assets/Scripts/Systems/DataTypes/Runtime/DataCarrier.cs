using Purrcifer.Data.Player;
using UnityEngine;
using Purrcifer.Data.Xml;
using Purrcifer.PlayerData;

public class DataCarrier : MonoBehaviour
{
    /// <summary>
    /// Singleton reference. 
    /// </summary>
    private static DataCarrier _instance;

    /// <summary>
    /// The current game data. 
    /// </summary>
    [SerializeField] private GameSaveFileRuntime _runtime;

    /// <summary>
    /// Returns the current instance of the data manager. 
    /// </summary>
    public static DataCarrier Instance => _instance;

    /// <summary>
    /// Returns the current instance of the GameSaveFileRuntime. 
    /// </summary>
    public static GameSaveFileRuntime RuntimeData => _instance._runtime.Copy();

    /// <summary>
    /// Returns the currently saved level from save data. 
    /// </summary>
    public static int SavedLevel => Instance._runtime.currentGameLevel;

    private void Start()
    {
        //Set singleton. 
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        //Load the players data.
        LoadData();
    }

    /// <summary>
    /// Load the data from file. 
    /// </summary>
    private void LoadData()
    {
        //Set the path of the file. 
        string path = XML_Serialization.AppDirPath + "GS.xml";
        bool fileExists = XML_Serialization.DataExists(XML_Serialization.AppDirPath + "GS.xml");

        //If the file doesn't exist create one, else load data. 
        if (!fileExists)
        {
            _runtime = GameSaveFileRuntime.Default();
        }
        else
        {
            GameSaveFileXML xml = XML_Serialization.Deserialize<GameSaveFileXML>(path);
            _runtime = (GameSaveFileRuntime)xml;
        }
    }

    /// <summary>
    /// Saves the data into the gs file. 
    /// </summary>
    private void SaveData()
    {
        string path = XML_Serialization.PersistDirPath + "GS.xml";
        XML_Serialization.CheckPathExists(XML_Serialization.PersistDirPath);
        XML_Serialization.Serialize<GameSaveFileXML>((GameSaveFileXML)_runtime, path);
    }

    /// <summary>
    /// Create a new instance of the DataCarrier. 
    /// </summary>
    /// <returns> reference to the new instance of the DataCarrier. </returns>
    public static DataCarrier Generate()
    {
        GameObject go = new GameObject("----Data----");
        return go.AddComponent<DataCarrier>();
    }

    /// <summary>
    /// Set the current player state.
    /// </summary>
    /// <param name="healthRange"> The health range to set. </param>
    /// <param name="damageData"> The damage data to set. </param>
    public void GetPlayerState(ref PlayerHealthRange healthRange, ref PlayerDamageData damageData)
    {
        healthRange = (PlayerHealthRange)_runtime;
        damageData = (PlayerDamageData)_runtime;
    }

    /// <summary>
    /// Pushes the current data into the save file. 
    /// </summary>
    /// <param name="state"> The state to set. </param>
    public void UpdatePlayerState(PlayerState state)
    {
        _runtime.SetPlayerHealthData(state);
        _runtime.SetPlayerDamageData(state);
        SaveData();
    }

    /// <summary>
    /// Used to reset the data currently attributed to the game. 
    /// </summary>
    public void ResetPlayerData()
    {
        _runtime = _runtime.GetDefaultPlayerData();
    }

    public void SetPlayerStats(PlayerStartingStatsSO startingStats)
    {
        _runtime.characterName = startingStats.characterName;
        _runtime.characterID = startingStats.characterID;
        _runtime.minHealth = startingStats.minHealth;
        _runtime.maxHealth = startingStats.maxHealth;
        _runtime.currentHealth = startingStats.currentHealth;

        _runtime.baseDamage = startingStats.baseDamage;
        _runtime.damageMultiplier = startingStats.damageMultiplier;
        _runtime.criticalHitDamage = startingStats.criticalHitDamage;
        _runtime.criticalHitChance = startingStats.criticalHitChance;

        _runtime.movementSpeed = startingStats.movementSpeed;
        _runtime.utilityCharges = startingStats.utilityCharges;
    }
}
