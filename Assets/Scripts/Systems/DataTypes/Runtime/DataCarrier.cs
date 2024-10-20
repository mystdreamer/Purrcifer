using Purrcifer.Data.Player;
using UnityEngine;
using Purrcifer.Data.Xml;

namespace DataManager
{
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
        public static GameSaveFileRuntime RuntimeData
        {
            get => _instance._runtime.Copy();
        }

        public static PlayerInputs PlayerInputs => Instance._runtime.playerInputs;

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
        /// Pushes the current data into the save file. 
        /// </summary>
        /// <param name="state"> The state to set. </param>
        public void SetPlayerData(PlayerState state)
        {
            GameManager.Instance.PlayerState.GetPlayerData(_runtime);
            SaveData();
        }

        public void SetPlayerData(PlayerStartingStatsSO startingStats)
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

        /// <summary>
        /// Used to reset the data currently attributed to the game. 
        /// </summary>
        public void ResetPlayerData() => _runtime = _runtime.GetDefaultPlayerData();
    }
}