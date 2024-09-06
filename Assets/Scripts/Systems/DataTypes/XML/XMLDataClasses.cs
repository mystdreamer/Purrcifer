using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
    #region Game Save File. 
    public class GameSaveFileXML
    {
        [XmlElement("playerData")]
        public PlayerDataXML playerData;

        [XmlElement("savedGameSettings")]
        public SettingDataXML settingData;

        [XmlElement("savedGameState")]
        public GameStateDataXML gameStateData;

        [XmlElement("savedGameState")]
        public GameEventDataXML eventData;
    }
    #endregion

    #region Events. 
    public class GameEventXML
    {
        [XmlElement("name")]
        public string name;
        [XmlElement("id")]
        public int id;
        [XmlElement("state")]
        public bool state;
    }

    public class GameEventDataXML
    {
        [XmlArray("events")]
        public GameEventXML[] events;
    }
    #endregion

    #region Player Data. 
    public class PlayerDataXML
    {
        ////////////////////
        // Player State Data
        ////////////////////
        [XmlElement("min_health")]
        public int min;
        [XmlElement("max_health")]
        public int max;
        [XmlElement("current_health")]
        public int current;
        [XmlElement("base_damage")]
        public float baseDamage;
        [XmlElement("damage_multiplier")]
        public float damageMultiplier;
        [XmlElement("critical_hit_damage")]
        public float criticalHitDamage;
        [XmlElement("_critical_hit_chance")]
        public float criticalHitChance;
    }
    #endregion

    #region Settings Data. 
    public class SettingDataXML
    {
        ////////////////////
        // Settings State Data
        ////////////////////
        [XmlElement("masterVolume")]
        public float masterVolume;
        [XmlElement("sfxVolume")]
        public float sfxVolume;
        [XmlElement("uiVolume")]
        public float uiVolume;
        [XmlElement("bgmVolume")]
        public float bgmVolume;
    }
    #endregion

    #region Game State Data. 
    public class GameStateDataXML
    {
        ////////////////////
        // Game State Data
        ////////////////////
        [XmlElement("currentLevel")]
        public int currentLevel;
    }
    #endregion


}
