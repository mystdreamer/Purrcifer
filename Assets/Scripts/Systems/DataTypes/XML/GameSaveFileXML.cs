using System.Xml.Serialization;
using UnityEngine;

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
        [XmlArray("_utility_Identifiers")]
        public int[] weaponIDs;
        [XmlArray("_weapon_Identifiers")]
        public int[] utilityIDs;
        [XmlElement("_master_volume")]
        public float masterVolume;
        [XmlElement("_sfx_volume")]
        public float sfxVolume;
        [XmlElement("_ui_volume")]
        public float uiVolume;
        [XmlElement("_bgm_volume")]
        public float bgmVolume;

        [XmlElement("_keyboard_up")]
        public int key_m_up;
        [XmlElement("_keyboard_down")]
        public int key_m_down;
        [XmlElement("_keyboard_right")]
        public int key_m_right;
        [XmlElement("_keyboard_left")]
        public int key_m_left;

        [XmlElement("_keyboard_action_up")]
        public int key_a_up;
        [XmlElement("_keyboard_action_down")]
        public int key_a_down;
        [XmlElement("_keyboard_action_right")]
        public int key_a_right;
        [XmlElement("_keyboard_action_left")]
        public int key_a_left;
        [XmlElement("_keyboard_menu_a")]
        public int key_menu_a;
        [XmlElement("_keyboard_util_action_a")]
        public int key_util_action_a;
        [XmlElement("_keyboard_util_action_b")]
        public int key_util_action_b;

        [XmlElement("_ctlr_a")]
        public int ctlr_a;
        [XmlElement("_ctlr_b")]
        public int ctlr_b;
        [XmlElement("_ctlr_x")]
        public int ctlr_x;
        [XmlElement("_ctlr_y")]
        public int ctlr_y;
        [XmlElement("_ctlr_util_action_a")]
        public int ctlr_util_action_a;
        [XmlElement("_ctlr_util_action_b")]
        public int ctlr_util_action_b;

        [XmlElement("_axis_m_left")]
        public int axis_m_left;
        [XmlElement("_axis_a_right")]
        public int axis_a_right;
        [XmlElement("_axis_d_pad")]
        public int axis_d_pad;
    }
}