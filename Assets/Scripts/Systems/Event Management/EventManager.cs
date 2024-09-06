using NUnit.Framework;
using Purrcifer.Data.Xml;
using UnityEditor.PackageManager;
using UnityEngine;

public struct EventData
{
    public string name;
    public int ID;
    public bool state;

    public EventData(string name, int id, bool value = false)
    {
        this.name = name;
        this.ID = id;
        this.state = value;
    }

    public static explicit operator GameEventXML(EventData data)
    {
        return new GameEventXML() { name = data.name, id = data.ID, state = data.state };
    }

    public static explicit operator EventData(GameEventXML data)
    {
        return new EventData(data.name, data.id, data.state);
    }
}

public class EventManager
{
    public EventData[] events;

    private EventManager(EventData[] eventData)
    {
        this.events = eventData;
    }

    public void SetState(int id, bool state)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].ID == id)
            {
                events[i].state = state;
            }
        }
    }

    public void SetState(string name, bool state)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].name == name)
            {
                events[i].state = state;
            }
        }
    }
}
