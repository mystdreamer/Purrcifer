using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    WALL,
    DOOR
}

/// <summary>
/// Component class handling the state of doors at runtime. 
/// </summary>
public class RoomDoorController : RoomObject
{
    /// <summary>
    /// The door heading upward. 
    /// </summary>
    public WallStateHandler up;

    /// <summary>
    /// The door heading downwards. 
    /// </summary>
    public WallStateHandler down;

    /// <summary>
    /// The door heading left. 
    /// </summary>
    public WallStateHandler left;

    /// <summary>
    /// The door heading right. 
    /// </summary>
    public WallStateHandler right;

    public bool doorsLocked = false;

    private void Start()
    {
        //Set the initial states for each wall. 
        up.SetState(WallState.WALL);
        down.SetState(WallState.WALL);
        left.SetState(WallState.WALL);
        right.SetState(WallState.WALL);
    }

    private void Update()
    {

    }

    public override void AwakenRoom()
    {
        base.AwakenRoom();
        LockRoom(true);
        base.Complete = true;
    }

    public override void SleepRoom()
    {
        base.SleepRoom();
        LockRoom(false);
    }

    /// <summary>
    /// Set the given side to be a door. 
    /// </summary>
    /// <param name="op"> The side to set. </param>
    public void SetDoorState(DoorOpenOp op)
    {
        if (op == DoorOpenOp.LEFT)
            left.SetState(WallState.DOOR);
        if (op == DoorOpenOp.RIGHT)
            right.SetState(WallState.DOOR);
        if (op == DoorOpenOp.UP)
            up.SetState(WallState.DOOR);
        if (op == DoorOpenOp.DOWN)
            down.SetState(WallState.DOOR);
    }

    /// <summary>
    /// Locks/Unlocks a given room. 
    /// </summary>
    /// <param name="state"> The flag indicating whether to lock or unlock (true = lock). </param>
    public void LockRoom(bool state)
    {
        if (up.isDoor)
            up.LockDoors(state);
        if (down.isDoor)
            down.LockDoors(state);
        if (right.isDoor)
            right.LockDoors(state);
        if (left.isDoor)
            left.LockDoors(state);
    }

    internal override void Event_Collision(GameObject collision)
    {
    }

    internal override void Event_Triggered(GameObject collision)
    {
    }

    internal override void SetWorldState(WorldStateEnum state)
    {
    }
}
