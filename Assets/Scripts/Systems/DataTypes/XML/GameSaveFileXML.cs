using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
    public class GameSaveFileXML
    {
        ////////////////////
        // Player State Data
        ////////////////////
        [XmlElement("character_name")]
        public string characterName;
        [XmlElement("character_id")]
        public int characterID;
        [XmlElement("min_health")]
        public int minHealth;
        [XmlElement("max_health")]
        public int maxHealth;
        [XmlElement("current_health")]
        public int currentHealth;
        [XmlElement("base_damage")]
        public float baseDamage;
        [XmlElement("damage_multiplier")]
        public float damageMultiplier;
        [XmlElement("critical_hit_damage")]
        public float criticalHitDamage;
        [XmlElement("_critical_hit_chance")]
        public float criticalHitChance;
        [XmlElement("_movement_speed")]
        public float movementSpeed;
        [XmlElement("_utility_charges")]
        public int utilityCharges;
        [XmlElement("_current_game_level")]
        public int currentGameLevel;
        [XmlElement("_master_volume")]
        public float masterVolume;
        [XmlElement("_sfx_volume")]
        public float sfxVolume;
        [XmlElement("_ui_volume")]
        public float uiVolume;
        [XmlElement("_bgm_volume")]
        public float bgmVolume;
    }
}