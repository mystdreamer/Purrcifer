using Purrcifer.Data.Defaults;
using UnityEngine;

public class Area_Pull : InZone
{
    public float force;
    public PlayerMovementSys playerMovement;

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
        if (playerMovement != null)
        {
            //Generate the vector between the two points. 
            Vector3 vectorBetween = transform.position - playerMovement.gameObject.transform.position; 

            Debug.Log("ForcePush: Applying direction.");
            playerMovement.body.AddForce(vectorBetween.normalized * force, ForceMode.Impulse);
        }
    }

    internal override void OnEnterZone() { }

    internal override void OnExitZone() { }

    internal override void SetWorldState(WorldStateEnum state)
    {
        //Apply world state changes. 
    }
}