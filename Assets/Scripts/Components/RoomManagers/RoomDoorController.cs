using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    WALL,
    DOOR
}

[System.Serializable]
public class WallStateHandler
{
    /// <summary>
    /// The door object reference. 
    /// </summary>
    public GameObject door; 
    
    /// <summary>
    /// The door wall object reference.
    /// </summary>
    public GameObject doorWall;
    
    /// <summary>
    /// The wall object reference. 
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// The state applied to this wall. 
    /// </summary>
    public WallState state;

    /// <summary>
    /// Boolean denoting if the wall is a door. 
    /// </summary>
    public bool isDoor;

    /// <summary>
    /// Changes the state of the wall. 
    /// </summary>
    /// <param name="state"></param>
    public void SetState(WallState state)
    {
        switch (state)
        {
            case WallState.DOOR:
                doorWall.SetActive(true);
                door.SetActive(false);
                wall.SetActive(false);
                this.state = state;
                isDoor = true;
                break;

            case WallState.WALL:
                doorWall.SetActive(false);
                door.SetActive(false);
                wall.SetActive(true);
                this.state = state;
                break;
        }

    }

    /// <summary>
    /// Set the lock state to the door. 
    /// </summary>
    /// <param name="lockState"> The state to set. </param>
    public void LockDoors(bool lockState)
    {
        if (isDoor)
        {
            door.SetActive(lockState);
        }
    }
}

/// <summary>
/// Component class handling the state of doors at runtime. 
/// </summary>
public class RoomDoorController : MonoBehaviour, IRoomInterface
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

    private void Start()
    {
        //Set the initial states for each wall. 
        up.SetState(WallState.WALL);
        down.SetState(WallState.WALL);
        left.SetState(WallState.WALL);
        right.SetState(WallState.WALL);
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

    void IRoomInterface.Awake()
    {
        LockRoom(true);
    }

    void IRoomInterface.Sleep()
    {
        LockRoom(false);
    }

    bool IRoomInterface.IsComplete()
    {
        return true; 
    }
}
