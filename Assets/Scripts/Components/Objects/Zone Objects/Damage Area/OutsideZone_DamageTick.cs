using Purrcifer.Data.Defaults;
using UnityEngine;

[RequireComponent(typeof(ObjectEventTicker))]
public class OutsideZone_DamageTick : OutsideZone
{
    public ObjectEventTicker eventTicker;

    private void OnEnable()
    {
        Ticker = eventTicker;
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        //Do world state setting stuff here. 
    }

    internal override void UpdateObject()
    {
        if (eventTicker.TickComplete)
        {
            GameManager.Instance.PlayerState.Health -= 1;
            eventTicker.TickComplete = false;
        }
    }
}

