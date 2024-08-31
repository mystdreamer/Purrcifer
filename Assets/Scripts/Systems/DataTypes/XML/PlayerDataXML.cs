using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
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
}