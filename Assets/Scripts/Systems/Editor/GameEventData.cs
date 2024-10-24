using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace GameEditor.GameEvent
{
    [System.Serializable]
    public class GameEventData
    {
        [XmlIgnore]
        public bool validated = false;
        [XmlIgnore]
        public bool validName = true;
        [XmlIgnore]
        public bool validID = true;
        
        public string name;
        public int id;
        public bool state;

        public string GetLabel(bool selected)
        {
            string prefix = (selected) ? "<color=yellow> -- </color>" : "";
            string suffix = (validated) ? "<color=yellow> [VALID] </color>" : "<color=red> [INVALID] </color>";
            return prefix + name + " [id: " + id + "]" + suffix;
        }

        public TextField GetTextField()
        {
            return new TextField("Event Name") { value = name };
        }

        public IntegerField GetIntField()
        {
            return new IntegerField("Event ID") { value = id };
        }

        public void Validate(GameEventData eventData)
        {
            validName = (name != eventData.name);
            validID = (id != eventData.id);
            validated = (validName && validID);
        }
    }
}

