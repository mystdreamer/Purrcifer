using Purrcifer.Data.Defaults;
using UnityEngine;

public class Area_Push : InZone
{
    public Vector3 pushDirection;
    public float force;
    public MovementSys playerMovement;

    internal override void Update()
    {
        base.Update();

        Debug.Log("ForcePush: Entered update.");

        if (!InZone)
            return; 

        ///If player reference doesn't exist, cache it. 
        if (playerMovement == null)
            playerMovement = GameManager.Instance.playerCurrent.GetComponent<MovementSys>();

        //Apply forces to the object. 
        if (playerMovement != null)
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
