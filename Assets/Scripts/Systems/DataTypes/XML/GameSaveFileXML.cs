using System.Xml.Serialization;

namespace Purrcifer.Data.Xml
{
    public class GameSaveFileXML
    {
        [XmlElement("playerData")]
        public PlayerDataXML playerData;

        [XmlElement("savedGameSettings")]
        public SettingDataXML settingData;

        [XmlElement("savedGameState")]
        public GameStateDataXML gameStateData;
    }
}