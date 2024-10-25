using Purrcifer.Data.Defaults;
using UnityEngine;

/// <summary>
/// Example area class which will tick damage over a period of time. 
/// </summary>
[RequireComponent(typeof(ObjectEventTicker))]
public class InZone_DamageOverTick : InZone
{
    /// <summary>
    /// The reference to the ObjectEventTicker. 
    /// </summary>
    public ObjectEventTicker eventTicker;

    private void OnEnable()
    {
        Ticker = eventTicker;
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        ///Here is where worlds state changes would be implemented. 
    }

    internal override void UpdateObject()
    {
        //Check if event tick has completed. 
        if (eventTicker.TickComplete)
        {
            //If so apply damage to the player. 
            GameManager.Instance.PlayerState.Health -= 1;
            eventTicker.TickComplete = false;
        }
    }
}
