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

    /// <summary>
    /// This is overridden, with the base start being called first for object scale updates. 
    /// </summary>
    internal override void Start()
    {
        //Update base size. 
        base.Start();

        //Cache reference if not already cached. 
        if (eventTicker == null)
            eventTicker = gameObject.GetComponent<ObjectEventTicker>();
    }

    /// <summary>
    /// This is overriden with base update being called first to handle object updating. 
    /// </summary>
    internal override void Update()
    {
        //Update base size. 
        base.Update();

        //Check if event tick has completed. 
        if (eventTicker.TickComplete)
        {
            //If so apply damage to the player. 
            GameManager.Instance.PlayerState.Health -= 1;
            eventTicker.TickComplete = false; 
        }
    }

    internal override void OnEnterZone()
    {
        //Used to debug collisions.  
        UnityEngine.Debug.Log(gameObject.name + ": -> Player is in zone. ");
        eventTicker.Enable = true;

    }

    internal override void OnExitZone()
    {
        //Used to debug collisions.  
        UnityEngine.Debug.Log(gameObject.name + ": -> Player left zone. ");
        eventTicker.Enable = false;
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {
        ///Here is where worlds state changes would be implemented. 
    }
}
