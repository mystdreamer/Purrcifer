using Purrcifer.Data.Defaults;

public class InZone_DamageOverTick : InZone
{
    public ObjectEventTicker eventTicker;

    internal override void Start()
    {
        base.Start();

        if (eventTicker == null)
            eventTicker = gameObject.GetComponent<ObjectEventTicker>();
    }

    internal override void Update()
    {
        base.Update();

        if (eventTicker.TickComplete)
        {
            GameManager.Instance.playerState.AddDamage = 1;
        }
    }

    internal override void SetWorldState(WorldStateEnum state) { 
    
    }

    internal override void OnEnterZone()
    {
        UnityEngine.Debug.Log(gameObject.name + ": -> Player is in zone. ");
        eventTicker.Enable = true;

    }

    internal override void OnExitZone()
    {
        UnityEngine.Debug.Log(gameObject.name + ": -> Player left zone. ");
        eventTicker.Enable = false;
    }
}
