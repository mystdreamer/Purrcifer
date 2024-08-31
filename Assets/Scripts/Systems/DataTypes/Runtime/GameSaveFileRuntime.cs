using Purrcifer.Data.Xml;

namespace Purrcifer.Data.Player
{
    [System.Serializable]
    public class GameSaveFileRuntime
    {
        public PlayerDataRuntime playerData;
        public SettingsDataRuntime settingData;
        public GameStateDataRuntime gameState;

        private GameSaveFileRuntime() { }

        public GameSaveFileRuntime(GameSaveFileXML dataFile)
        {
            playerData = new PlayerDataRuntime(dataFile.playerData);
            settingData = new SettingsDataRuntime(dataFile.settingData);
            gameState = new GameStateDataRuntime(dataFile.gameStateData);
        }

        public static GameSaveFileRuntime GetDefault()
        {
            return new GameSaveFileRuntime()
            {
                playerData = PlayerDataRuntime.GetDefault(),
                settingData = SettingsDataRuntime.GetDefault(),
                gameState = GameStateDataRuntime.GetDefault()
            };
        }

        public static explicit operator GameSaveFileXML(GameSaveFileRuntime data)
        {
            return new GameSaveFileXML()
            {
                playerData = (PlayerDataXML)data.playerData,
                settingData = (SettingDataXML)data.settingData,
                gameStateData = (GameStateDataXML)data.gameState
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
            min = 0,
            max = 3,
            current = 3,
            baseDamage = 1,
            damageMultiplier = 1,
            criticalHitDamage = 3,
            criticalHitChance = 10,
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
                currentLevel = 0
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
                masterVolume = 50,
                sfxVolume = 50,
                uiVolume = 50,
                bgmVolume = 50
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
}
