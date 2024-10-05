using DataManager;
using Purrcifer.Data.Defaults;
using Purrcifer.Data.Xml;
using Purrcifer.PlayerData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Purrcifer.Data.Player
{

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
                playerInputs = new PlayerInputs() {
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
