using Purrcifer.Data.Defaults;
using UnityEngine;

[RequireComponent(typeof(ObjectEventTicker))]
public class Area_PushWave : InZone
{
    public ObjectEventTicker objectEventTicker;
    public Vector3 pushDirection;
    public float force;
    public PlayerMovementSys playerMovement;

    internal override void Start()
    {
        base.Start();
        objectEventTicker.Enable = true;
    }

    internal override void Update()
    {
        base.Update();

        Debug.Log("ForcePush: Entered update.");

        if (!InZone)
            return;

        ///If player reference doesn't exist, cache it. 
        if (playerMovement == null)
            playerMovement = GameManager.Instance.PlayerMovementSys;

        //Apply forces to the object. 
        if (playerMovement != null && objectEventTicker.TickComplete)
        {
            Debug.Log("ForcePush: Applying direction.");
            playerMovement.body.AddForce(pushDirection.normalized * force, ForceMode.Impulse);
        }
    }

    internal override void OnEnterZone() { }

    internal override void OnExitZone() { }

    internal override void SetWorldState(WorldStateEnum state)
    {
        //Apply world state changes. 
    }
}