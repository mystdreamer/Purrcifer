using Purrcifer.Data.Defaults;
using UnityEngine;

[RequireComponent(typeof(ObjectEventTicker))]
public class OutsideZone_DamageTick : OutsideZone
{
    public ObjectEventTicker eventTicker;

    internal override void Start()
    {
        base.Start();

        if(eventTicker == null)
            eventTicker = gameObject.GetComponent<ObjectEventTicker>();
    }

    internal override void Update()
    {
        base.Update();

        if (eventTicker.TickComplete)
        {
            GameManager.Instance.PlayerState.AddDamage = 1; 
            eventTicker.TickComplete = false;
        }
    }

    internal override void OnEnterZone()
    {
        eventTicker.Enable = false;
    }

    internal override void OnExitZone()
    {
        eventTicker.Enable = true;
    }

    internal override void SetWorldState(WorldStateEnum state)
    {
        //Do world state setting stuff here. 
    }
}

