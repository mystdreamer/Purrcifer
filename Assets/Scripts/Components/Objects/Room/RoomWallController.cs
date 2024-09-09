using Purrcifer.Data.Defaults;
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
public class RoomWallController : RoomObjectBase
{
    /// <summary>
    /// The door heading upward. 
    /// </summary>
    public RoomWallData up;

    /// <summary>
    /// The door heading downwards. 
    /// </summary>
    public RoomWallData down;

    /// <summary>
    /// The door heading left. 
    /// </summary>
    public RoomWallData left;

    /// <summary>
    /// The door heading right. 
    /// </summary>
    public RoomWallData right;

    private void Start()
    {
        //Set the initial states for each wall. 
        up.WallType = WallState.WALL;
        down.WallType = WallState.WALL;
        left.WallType = WallState.WALL;
        right.WallType = WallState.WALL;
    }

    internal override void OnAwakeObject()
    {
        LockRoom(true);
        base.ObjectComplete = true;
    }

    internal override void OnSleepObject()
    {
        LockRoom(false);
    }

    internal override void SetWorldState(WorldStateEnum state) { }

    private void Update()
    {

    }

    /// <summary>
    /// Set the given side to be a door. 
    /// </summary>
    /// <param name="direction"> The side to set. </param>
    public void SetDoorState(WallDirection direction)
    {
        switch (direction)
        {
            case WallDirection.LEFT:
                left.WallType = WallState.DOOR;
                break;
            case WallDirection.RIGHT:
                right.WallType = WallState.DOOR;
                break;
            case WallDirection.UP:
                up.WallType = WallState.DOOR;
                break;
            case WallDirection.DOWN:
                down.WallType = WallState.DOOR;
                break;
        }
    }

    /// <summary>
    /// Locks/Unlocks a given room. 
    /// </summary>
    /// <param name="state"> The flag indicating whether to lock or unlock (true = lock). </param>
    public void LockRoom(bool state)
    {
        up.SetDoorLockState = state;
        down.SetDoorLockState = state;
        right.SetDoorLockState = state;
        left.SetDoorLockState = state;
    }
}
