using Purrcifer.Data.Defaults;
using Purrcifer.Data.Xml;
using Purrcifer.PlayerData;
using Purrcifer.PlayerDataCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Purrcifer.LevelLoading;

namespace Purrcifer.Data.Player
{
    [System.Serializable]
    public class PlayerGameEventData
    {
        [System.Serializable]
        public class GameEventData
        {
            public string name;
            public int id;
            public bool state;
        }

        [System.Serializable]
        public class GameEventDataWrapper
        {
            public GameEventData[] events;
        }

        public GameEventDataWrapper playerEvents;

        private string BaseEventPath => Application.streamingAssetsPath + "/Data/D_E.xml";
        private string DefaultEventPath => XML_Serialization.PersistPath + XML_Serialization.DefaultEventXML;
        private string SavedEventPath => XML_Serialization.PersistPath + XML_Serialization.SavedEventXML;

        public PlayerGameEventData()
        {
            LoadEvents();
        }

        public void LoadEvents()
        {
            GameEventDataWrapper wrapper;

            //Load the saved version of the events.
            wrapper = XML_Serialization.AttemptDeserialization<GameEventDataWrapper>(
                XML_Serialization.PersistPath,
                XML_Serialization.SavedEventXML,
                out bool succeedESF
                );
            if (succeedESF)
            {
                playerEvents = wrapper;
                return;
            }

            //Load default version of the events.
            CopyDefaultEvents();

            wrapper = XML_Serialization.AttemptDeserialization<GameEventDataWrapper>(
                XML_Serialization.PersistPath,
                XML_Serialization.DefaultEventXML,
                out bool succeedDE
                );
            if (succeedDE)
            {
                playerEvents = wrapper;
                SaveEvents();
                return;
            }

            throw new System.Exception("No default event data");
        }

        public void SaveEvents()
        {
            GameEventDataWrapper gedw = playerEvents;
            XML_Serialization.AssureDirectoryExists(XML_Serialization.PersistPath);
            XML_Serialization.Serialize<GameEventDataWrapper>(gedw, SavedEventPath);
            Debug.Log("Event serialization path: " + SavedEventPath);
        }

        private void CopyDefaultEvents()
        {
            Debug.Log(Application.streamingAssetsPath);
            string path = BaseEventPath;
            string newFilePath = DefaultEventPath;
            File.Copy(path, newFilePath, true);
        }

        public bool GetEventState(string name)
        {
            GameEventData data = GetEvent(name);

            if (data != null) return data.state;
            else Debug.Log("Event Entry[" + name + "] could not be found");
            return false;
        }

        public bool GetEventState(int eventID)
        {
            GameEventData data = GetEvent(eventID);

            if (data != null) return data.state;
            else Debug.Log("Event Entry[" + eventID + "] could not be found");
            return false;
        }

        public List<GameEventData> GetCompletedEvents()
        {
            List<GameEventData> completed = new List<GameEventData>();

            for (int i = 0; i < playerEvents.events.Length; i++)
            {
                if (playerEvents.events[i].state)
                    completed.Add(playerEvents.events[i]);
            }

            return completed;
        }

        public List<GameEventData> GetIncompleteEvents()
        {
            List<GameEventData> completed = new List<GameEventData>();

            for (int i = 0; i < playerEvents.events.Length; i++)
            {
                if (!playerEvents.events[i].state)
                    completed.Add(playerEvents.events[i]);
            }

            return completed;
        }

        public void SetEventState(string name, bool state)
        {
            GameEventData data = GetEvent(name);

            if (data != null)
            {
                data.state = state;
            }
            else
                Debug.Log("Event Entry[" + name + "] could not be found");
        }

        public void SetEventState(int eventID, bool state)
        {
            GameEventData data = GetEvent(eventID);

            if (data != null)
            {
                data.state = state;
            }
            else
                Debug.Log("Event Entry[" + eventID + "] could not be found");
        }

        public GameEventData GetEvent(string name)
        {
            for (int i = 0; i < playerEvents.events.Length; i++)
            {
                if (playerEvents.events[i].name == name)
                    return playerEvents.events[i];
            }
            return null;
        }

        public GameEventData GetEvent(int id)
        {
            for (int i = 0; i < playerEvents.events.Length; i++)
            {
                if (playerEvents.events[i].id == id)
                    return playerEvents.events[i];
            }
            return null;
        }
    }

    [System.Serializable]
    public class GameSaveFileRuntime
    {
        public string characterName;
        public int characterID;
        public int minHealth;
        public int maxHealth;
        public int currentHealth;
        public float baseDamage;
        public float damageMultiplier;
        public float criticalHitDamage;
        public float criticalHitChance;
        public float movementSpeed;
        public float attackRate;
        public int utilityCharges;
        public int talismanCount;
        public int currentGameLevel;
        public List<int> utilityIds;
        public List<int> weaponIds;

        public PlayerInputs playerInputs;
        public float masterVolume;
        public float sfxVolume;
        public float uiVolume;
        public float bgmVolume;

        private GameSaveFileRuntime() { }

        public GameSaveFileRuntime Copy()
        {
            return new GameSaveFileRuntime()
            {
                characterName = characterName,
                characterID = characterID,
                minHealth = minHealth,
                maxHealth = maxHealth,
                currentHealth = currentHealth,
                baseDamage = baseDamage,
                damageMultiplier = damageMultiplier,
                criticalHitDamage = criticalHitDamage,
                criticalHitChance = criticalHitChance,
                movementSpeed = movementSpeed,
                utilityCharges = utilityCharges,
                talismanCount = talismanCount,
                currentGameLevel = currentGameLevel,
                utilityIds = utilityIds,
                weaponIds = weaponIds,
                masterVolume = masterVolume,
                sfxVolume = sfxVolume,
                uiVolume = uiVolume,
                bgmVolume = bgmVolume,
            };
        }

        #region Defaulting Functions.
        public GameSaveFileRuntime GetDefaultPlayerData()
        {
            return new GameSaveFileRuntime()
            {
                characterName = "",
                characterID = -1,
                minHealth = 0,
                maxHealth = 0,
                currentHealth = 0,
                baseDamage = 0,
                damageMultiplier = 0,
                criticalHitDamage = 0,
                criticalHitChance = 0,
                movementSpeed = 0,
                utilityCharges = 0,
                talismanCount = 0,
                currentGameLevel = DefaultGameStateData.CURRENT_LEVEL,
                utilityIds = new List<int>(),
                weaponIds = new List<int>(),
                playerInputs = this.playerInputs,
                masterVolume = this.masterVolume,
                sfxVolume = this.sfxVolume,
                uiVolume = this.uiVolume,
                bgmVolume = this.bgmVolume
            };
        }

        public static GameSaveFileRuntime Default()
        {
            return new GameSaveFileRuntime()
            {
                characterName = "",
                characterID = -1,
                minHealth = 0,
                maxHealth = 0,
                currentHealth = 0,
                baseDamage = 0,
                damageMultiplier = 0,
                criticalHitDamage = 0,
                criticalHitChance = 0,
                movementSpeed = 0,
                utilityCharges = 0,
                talismanCount = 0,
                currentGameLevel = DefaultGameStateData.CURRENT_LEVEL,
                utilityIds = new List<int>(),
                weaponIds = new List<int>(),
                playerInputs = PlayerInputs.GetDefault(),
                masterVolume = DefaultSettingsData.MASTER_VOLUME,
                sfxVolume = DefaultSettingsData.SFX_VOLUME,
                uiVolume = DefaultSettingsData.UI_VOLUME,
                bgmVolume = DefaultSettingsData.BGM_VOLUME,
            };
        }
        #endregion

        public static GameSaveFileRuntime LoadData()
        {
            GameSaveFileRuntime runtime = new GameSaveFileRuntime();
            bool defaulted = false;

            //Set the path of the file. 
            string path = XML_Serialization.PersistPath + XML_Serialization.gameSaveFileName;
            bool fileExists = XML_Serialization.DataExists(path);

            //If the file doesn't exist create one, else load data. 
            if (!fileExists)
            {
                defaulted = true;
                runtime = GameSaveFileRuntime.Default();
            }
            else
            {
                GameSaveFileXML xml = XML_Serialization.Deserialize<GameSaveFileXML>(path);
                runtime = (GameSaveFileRuntime)xml;
            }

            if (defaulted)
                runtime.SaveData();

            return runtime;
        }

        public void UpdateData()
        {
            if (GameManager.Instance.PlayerState != null)
            {
                PlayerState state = GameManager.Instance.PlayerState;

                maxHealth = state.HealthMaxCap;
                currentHealth = state.Health;
                baseDamage = state.BaseDamage;
                damageMultiplier = state.DamageMultiplier;
                criticalHitDamage = state.CriticalHitDamage;
                criticalHitChance = state.CriticalHitChance;
                movementSpeed = state.MovementSpeed;
                attackRate = state.AttackRate;
                talismanCount = state.Talismans;
            }
        }

        public void SaveData()
        {
            if(GameManager.Instance.PlayerState != null)
            {
                PlayerState state = GameManager.Instance.PlayerState;

                maxHealth = state.HealthMaxCap;
                currentHealth = state.Health;
                baseDamage = state.BaseDamage;
                damageMultiplier = state.DamageMultiplier;
                criticalHitDamage = state.CriticalHitDamage;
                criticalHitChance = state.CriticalHitChance;
                movementSpeed = state.MovementSpeed;
                attackRate = state.AttackRate;
                talismanCount = state.Talismans;
            }

            string path = XML_Serialization.PersistPath + XML_Serialization.gameSaveFileName;
            XML_Serialization.AssureDirectoryExists(XML_Serialization.PersistPath);
            XML_Serialization.Serialize<GameSaveFileXML>((GameSaveFileXML)this, path);
        }

        public static explicit operator GameSaveFileXML(GameSaveFileRuntime data)
        {
            return new GameSaveFileXML()
            {
                characterName = data.characterName,
                characterID = data.characterID,
                minHealth = data.minHealth,
                maxHealth = data.maxHealth,
                currentHealth = data.currentHealth,
                baseDamage = data.baseDamage,
                damageMultiplier = data.damageMultiplier,
                criticalHitDamage = data.criticalHitDamage,
                criticalHitChance = data.criticalHitChance,
                movementSpeed = data.movementSpeed,
                talismanCount = data.talismanCount,
                utilityCharges = data.utilityCharges,
                currentGameLevel = data.currentGameLevel,
                utilityIDs = data.utilityIds.ToArray(),
                weaponIDs = data.weaponIds.ToArray(),
                masterVolume = data.masterVolume,
                sfxVolume = data.sfxVolume,
                uiVolume = data.uiVolume,
                bgmVolume = data.bgmVolume,
                key_m_up = (int)data.playerInputs.key_m_up,
                key_m_down = (int)data.playerInputs.key_m_down,
                key_m_right = (int)data.playerInputs.key_m_right,
                key_m_left = (int)data.playerInputs.key_m_left,
                key_a_up = (int)data.playerInputs.key_a_up,
                key_a_down = (int)data.playerInputs.key_a_down,
                key_a_right = (int)data.playerInputs.key_a_right,
                key_a_left = (int)data.playerInputs.key_a_left,
                key_menu_a = (int)data.playerInputs.key_menu_a,
                key_util_action_a = (int)data.playerInputs.key_util_action_a,
                key_util_action_b = (int)data.playerInputs.key_util_action_b,
                ctlr_a = (int)data.playerInputs.ctlr_a,
                ctlr_b = (int)data.playerInputs.ctlr_b,
                ctlr_x = (int)data.playerInputs.ctlr_x,
                ctlr_y = (int)data.playerInputs.ctlr_y,
                ctlr_util_action_a = (int)data.playerInputs.ctlr_util_action_a,
                ctlr_util_action_b = (int)data.playerInputs.ctlr_util_action_b,
                axis_m_left = (int)data.playerInputs.axis_m_left,
                axis_a_right = (int)data.playerInputs.axis_a_right,
                axis_d_pad = (int)data.playerInputs.axis_d_pad,
            };
        }

        public static explicit operator GameSaveFileRuntime(GameSaveFileXML data)
        {
            return new GameSaveFileRuntime()
            {
                characterName = data.characterName,
                characterID = data.characterID,
                minHealth = data.minHealth,
                maxHealth = data.maxHealth,
                currentHealth = data.currentHealth,
                baseDamage = data.baseDamage,
                damageMultiplier = data.damageMultiplier,
                criticalHitDamage = data.criticalHitDamage,
                criticalHitChance = data.criticalHitChance,
                movementSpeed = data.movementSpeed,
                utilityCharges = data.utilityCharges,
                talismanCount = data.talismanCount,
                currentGameLevel = data.currentGameLevel,
                utilityIds = data.utilityIDs.ToList(),
                weaponIds = data.weaponIDs.ToList(),
                masterVolume = data.masterVolume,
                sfxVolume = data.sfxVolume,
                uiVolume = data.uiVolume,
                bgmVolume = data.bgmVolume,
                playerInputs = new PlayerInputs()
                {
                    key_m_up = (KeyCode)data.key_m_up,
                    key_m_down = (KeyCode)data.key_m_down,
                    key_m_right = (KeyCode)data.key_m_right,
                    key_m_left = (KeyCode)data.key_m_left,
                    key_a_up = (KeyCode)data.key_a_up,
                    key_a_down = (KeyCode)data.key_a_down,
                    key_a_right = (KeyCode)data.key_a_right,
                    key_a_left = (KeyCode)data.key_a_left,
                    key_menu_a = (KeyCode)data.key_menu_a,
                    key_util_action_a = (KeyCode)data.key_util_action_a,
                    key_util_action_b = (KeyCode)data.key_util_action_b,
                    ctlr_a = (KeyCode)data.ctlr_a,
                    ctlr_b = (KeyCode)data.ctlr_b,
                    ctlr_x = (KeyCode)data.ctlr_x,
                    ctlr_y = (KeyCode)data.ctlr_y,
                    ctlr_util_action_a = (KeyCode)data.ctlr_util_action_a,
                    ctlr_util_action_b = (KeyCode)data.ctlr_util_action_b,
                    axis_m_left = (PInputIdentifier)data.axis_m_left,
                    axis_a_right = (PInputIdentifier)data.axis_a_right,
                    axis_d_pad = (PInputIdentifier)data.axis_d_pad,
                }
            };
        }

        public static explicit operator PlayerDamageData(GameSaveFileRuntime data)
        {
            return new PlayerDamageData(
                data.baseDamage,
                data.damageMultiplier,
                data.criticalHitDamage,
                data.criticalHitChance);
        }

        public static explicit operator PlayerHealthData(GameSaveFileRuntime data)
        {
            return new PlayerHealthData(data.minHealth, data.maxHealth, data.currentHealth);
        }
    }
}
