using Purrcifer.Data.Xml;
using Purrcifer.Data.Defaults;
using System.Collections.Generic;

namespace Purrcifer.Data.Player
{
    [System.Serializable]
    public class GameSaveFileRuntime
    {
        public PlayerDataRuntime playerData;
        public SettingsDataRuntime settingData;
        public GameStateDataRuntime gameState;
        public GameEventsRuntime gameEvents;

        private GameSaveFileRuntime() { }

        public GameSaveFileRuntime(GameSaveFileXML dataFile)
        {
            playerData = new PlayerDataRuntime(dataFile.playerData);
            settingData = new SettingsDataRuntime(dataFile.settingData);
            gameState = new GameStateDataRuntime(dataFile.gameStateData);
            gameEvents = new GameEventsRuntime(dataFile.eventData);
        }

        public GameSaveFileRuntime Copy()
        {
            return new GameSaveFileRuntime()
            {
                playerData = playerData.Copy(),
                settingData = settingData.Copy(),
                gameState = gameState.Copy(),
                gameEvents = gameEvents.Copy()
            };
        }

        public static GameSaveFileRuntime GetDefault()
        {
            return new GameSaveFileRuntime()
            {
                playerData = PlayerDataRuntime.GetDefault(),
                settingData = SettingsDataRuntime.GetDefault(),
                gameState = GameStateDataRuntime.GetDefault(),
                gameEvents = GameEventsRuntime.GetDefault()
            };
        }

        public static explicit operator GameSaveFileXML(GameSaveFileRuntime data)
        {
            return new GameSaveFileXML()
            {
                playerData = (PlayerDataXML)data.playerData,
                settingData = (SettingDataXML)data.settingData,
                gameStateData = (GameStateDataXML)data.gameState,
                eventData = (GameEventDataXML)data.gameEvents
            };
        }
    }

    [System.Serializable]
    public class PlayerDataRuntime
    {
        public int min;
        public int max;
        public int current;
        public float baseDamage;
        public float damageMultiplier;
        public float criticalHitDamage;
        public float criticalHitChance;

        private PlayerDataRuntime() { }

        public static PlayerDataRuntime GetDefault()
        {
            return new PlayerDataRuntime()
            {
                min = PlayerDefaultData.MIN_HEALTH,
                max = PlayerDefaultData.MAX_HEALTH,
                current = PlayerDefaultData.CURRENT_HEALTH,
                baseDamage = PlayerDefaultData.BASE_DAMAGE,
                damageMultiplier = PlayerDefaultData.BASE_MULTIPLIER,
                criticalHitDamage = PlayerDefaultData.CRITICAL_HIT_DAMAGE,
                criticalHitChance = PlayerDefaultData.CRITICAL_HIT_CHANCE,
            };
        }

        public PlayerDataRuntime(PlayerDataXML dataFile)
        {
            min = dataFile.min;
            max = dataFile.max;
            current = dataFile.current;
            baseDamage = dataFile.baseDamage;
            damageMultiplier = dataFile.damageMultiplier;
            criticalHitDamage = dataFile.criticalHitChance;
            criticalHitChance = dataFile.criticalHitChance;
        }

        public PlayerDataRuntime Copy()
        {
            return new PlayerDataRuntime()
            {
                min = this.min,
                max = this.max,
                current = this.current,
                baseDamage = this.baseDamage,
                damageMultiplier = this.damageMultiplier,
                criticalHitDamage = this.criticalHitChance,
                criticalHitChance = this.criticalHitChance,
            };
        }

        public static explicit operator PlayerDataXML(PlayerDataRuntime dataFile)
        {
            PlayerDataXML xml = new PlayerDataXML()
            {
                min = dataFile.min,
                max = dataFile.max,
                current = dataFile.current,
                baseDamage = dataFile.baseDamage,
                damageMultiplier = dataFile.damageMultiplier,
                criticalHitDamage = dataFile.criticalHitChance,
                criticalHitChance = dataFile.criticalHitChance
            };
            return xml;
        }
    }

    [System.Serializable]
    public class GameStateDataRuntime
    {
        public int currentLevel;

        private GameStateDataRuntime() { }

        public GameStateDataRuntime(GameStateDataXML dataFile)
        {
            currentLevel = dataFile.currentLevel;
        }

        public static GameStateDataRuntime GetDefault()
        {
            return new GameStateDataRuntime()
            {
                currentLevel = DefaultGameStateData.CURRENT_LEVEL
            };
        }

        public GameStateDataRuntime Copy()
        {
            return new GameStateDataRuntime()
            {
                currentLevel = this.currentLevel
            };
        }

        public static explicit operator GameStateDataXML(GameStateDataRuntime dataFile)
        {
            GameStateDataXML xml = new GameStateDataXML()
            {
                currentLevel = dataFile.currentLevel
            };
            return xml;
        }
    }

    [System.Serializable]
    public class SettingsDataRuntime
    {
        public float masterVolume;
        public float sfxVolume;
        public float uiVolume;
        public float bgmVolume;

        private SettingsDataRuntime() { }

        public SettingsDataRuntime(SettingDataXML dataFile)
        {
            masterVolume = dataFile.masterVolume;
            sfxVolume = dataFile.sfxVolume;
            uiVolume = dataFile.uiVolume;
            bgmVolume = dataFile.bgmVolume;
        }

        public static SettingsDataRuntime GetDefault()
        {
            return new SettingsDataRuntime()
            {
                masterVolume = DefaultSettingsData.MASTER_VOLUME,
                sfxVolume = DefaultSettingsData.SFX_VOLUME,
                uiVolume = DefaultSettingsData.UI_VOLUME,
                bgmVolume = DefaultSettingsData.BGM_VOLUME
            };
        }

        public SettingsDataRuntime Copy()
        {
            return new SettingsDataRuntime()
            {
                masterVolume = this.masterVolume,
                sfxVolume = this.sfxVolume,
                uiVolume = this.uiVolume,
                bgmVolume = this.bgmVolume
            };
        }

        public static explicit operator SettingDataXML(SettingsDataRuntime dataFile)
        {
            SettingDataXML xml = new SettingDataXML()
            {
                masterVolume = dataFile.masterVolume,
                sfxVolume = dataFile.sfxVolume,
                uiVolume = dataFile.uiVolume,
                bgmVolume = dataFile.bgmVolume
            };
            return xml;
        }
    }

    public class GameEventsRuntime
    {
        public EventData[] events;

        private GameEventsRuntime()
        {
        }

        public GameEventsRuntime(GameEventDataXML data)
        {

            List<EventData> _events = new List<EventData>();

            for (int i = 0; i < data.events.Length; i++)
                _events.Add((EventData)data.events[i]);

            events = _events.ToArray();
        }

        public static GameEventsRuntime GetDefault()
        {
            GameEventsRuntime runtime = new GameEventsRuntime();
            runtime.events = DefaultEventData.GetDefaultData();
            return runtime;
        }

        internal GameEventsRuntime Copy()
        {
            return new GameEventsRuntime() { events = this.events };
        }

        public static explicit operator GameEventsRuntime(GameEventDataXML data)
        {
            return new GameEventsRuntime(data);
        }

        public static explicit operator GameEventDataXML(GameEventsRuntime data)
        {
            GameEventXML[] _arr = new GameEventXML[data.events.Length];

            for (int i = 0; (i < data.events.Length); i++)
            {
                _arr[i] = (GameEventXML)data.events[i];
            }

            return new GameEventDataXML() { events = _arr };
        }
    }
}
