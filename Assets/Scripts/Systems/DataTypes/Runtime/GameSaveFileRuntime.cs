using Purrcifer.Data.Defaults;
using Purrcifer.Data.Xml;
using Purrcifer.PlayerData;
using System.Collections.Generic;

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
        public int currentGameLevel;
        public float masterVolume;
        public float sfxVolume;
        public float uiVolume;
        public float bgmVolume;
        public List<int> collectedWeaponIdentifiers; 
        public List<int> collectedUtilityIdentifiers; 

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
                currentGameLevel = currentGameLevel,
                masterVolume = masterVolume,
                sfxVolume = sfxVolume,
                uiVolume = uiVolume,
                bgmVolume = bgmVolume,
            };
        }

        public void SetPlayerHealthData(PlayerState data)
        {
            minHealth = data.HealthMinCap;
            maxHealth = data.HealthMaxCap;
            currentHealth = data.Health;
        }

        public void SetPlayerDamageData(PlayerState data)
        {
            baseDamage = data.Damage.BaseDamage;
            damageMultiplier = data.Damage.DamageMultiplier;
            criticalHitDamage = data.Damage.CriticalHitDamage;
            criticalHitChance = data.Damage.CriticalHitChance;
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
                currentGameLevel = DefaultGameStateData.CURRENT_LEVEL,
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
                currentGameLevel = DefaultGameStateData.CURRENT_LEVEL,
                masterVolume = DefaultSettingsData.MASTER_VOLUME,
                sfxVolume = DefaultSettingsData.SFX_VOLUME,
                uiVolume = DefaultSettingsData.UI_VOLUME,
                bgmVolume = DefaultSettingsData.BGM_VOLUME
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
                utilityCharges = data.utilityCharges,
                currentGameLevel = data.currentGameLevel,
                masterVolume = data.masterVolume,
                sfxVolume = data.sfxVolume,
                uiVolume = data.uiVolume,
                bgmVolume = data.bgmVolume,
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
                currentGameLevel = data.currentGameLevel,
                masterVolume = data.masterVolume,
                sfxVolume = data.sfxVolume,
                uiVolume = data.uiVolume,
                bgmVolume = data.bgmVolume,
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

        public static explicit operator PlayerHealthRange(GameSaveFileRuntime data)
        {
            return new PlayerHealthRange(data.minHealth, data.maxHealth, data.currentHealth);
        }
    }
}
