using Purrcifer.Data.Defaults;
using Room.WallController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component class handling the state of doors at runtime. 
/// </summary>
public class RoomWallController : RoomObjectBase
{
    private WallType RoomMarkerType;

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

    /// <summary>
    /// Lock/Unlock the room. 
    /// </summary>
    public bool SetLockState
    {
        set
        {
            up.SetDoorLockState = value;
            down.SetDoorLockState = value;
            right.SetDoorLockState = value;
            left.SetDoorLockState = value;
        }
    }

    public WallType MarkerType
    {
        get => RoomMarkerType;
        set
        {
            RoomMarkerType = value; 
            right.WallType = value;
            left.WallType = value;
            up.WallType = value;
            down.WallType = value;
        }
    }

    internal override void OnAwakeObject()
    {
        SetLockState = true;
        base.ObjectComplete = true;
    }

    internal override void OnSleepObject()
    {
        SetLockState = false;
    }

    /// <summary>
    /// Set the given side to be a door. 
    /// </summary>
    /// <param name="direction"> The side to set. </param>
    public void SetRoomState(WallDirection direction, WallType type)
    {
        switch (direction)
        {
            case WallDirection.LEFT:
                left.WallType = type;
                break;
            case WallDirection.RIGHT:
                right.WallType = type;
                break;
            case WallDirection.UP:
                up.WallType = type;
                break;
            case WallDirection.DOWN:
                down.WallType = type;
                break;
        }
    }

    internal override void WorldUpdateReceiver(WorldState state)
    {

    }
}
