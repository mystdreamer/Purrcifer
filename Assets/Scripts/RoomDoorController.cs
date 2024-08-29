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
    public GameObject doorState;
    public GameObject wallState;
    public WallState state;

    public bool isDoor;

    public void SetState(WallState state)
    {
        switch (state)
        {
            case WallState.DOOR:
                doorState.SetActive(true);
                wallState.SetActive(false);
                this.state = state;
                isDoor = true;
                break;

            case WallState.WALL:
                doorState.SetActive(false);
                wallState.SetActive(true);
                this.state = state;
                break;
        }

    }

    public void LockDoors(bool lockState)
    {
        if (isDoor)
        {
            doorState.SetActive(lockState);
        }
    }
}

public class RoomDoorController : MonoBehaviour
{
    public WallStateHandler up;
    public WallStateHandler down;
    public WallStateHandler left;
    public WallStateHandler right;

    private void Start()
    {
        up.SetState(WallState.WALL);
        down.SetState(WallState.WALL);
        left.SetState(WallState.WALL);
        right.SetState(WallState.WALL);
    }

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
}
