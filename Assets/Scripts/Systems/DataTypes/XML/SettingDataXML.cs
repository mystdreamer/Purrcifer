using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
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

    public class GameStateDataXML
    {
        ////////////////////
        // Game State Data
        ////////////////////
        [XmlElement("currentLevel")]
        public int currentLevel;
    }
}