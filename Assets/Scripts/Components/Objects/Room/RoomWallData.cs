using UnityEngine;

/// <summary>
/// Structure class representing a set of doors. 
/// </summary>
[System.Serializable]
public struct DoorSet
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _wall;

    public void SetWallState(WallState state)
    {
        switch (state)
        {
            case WallState.WALL:
                _door.SetActive(false);
                _wall.SetActive(false);
                break;
            case WallState.DOOR:
                _door.SetActive(false);
                _wall.SetActive(true);
                break;
        }
    }

    public void SetLockState(bool state)
    {
        _door.SetActive(state);
    }
}

/// <summary>
/// Class controlling the type of wall assigned to a room. 
/// </summary>
public class RoomWallData : MonoBehaviour
{
    [SerializeField] private WallState wallState = WallState.DOOR;

    /// <summary>
    /// The wall object. 
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// The set of door objects
    /// </summary>
    public DoorSet doorSet;

    /// <summary>
    /// Returns whether room set as a door. 
    /// </summary>
    public bool IsDoor => wallState == WallState.DOOR;

    /// <summary>
    /// The type of wall this is assigned to be. 
    /// </summary>
    public WallState WallType
    {
        get => wallState;
        set
        {
            this.wallState = value;
            SetState();
        }
    }

    public bool SetDoorLockState
    {
        set
        {
            if(IsDoor)
                doorSet.SetLockState(value);
        }
    }

    public void Start()
    {
        //Set the inital state of the room. 
        SetState();
    }

    /// <summary>
    /// Function used to manage the updating of wall states. 
    /// </summary>
    private void SetState()
    {
        switch (wallState)
        {
            case WallState.WALL:
                wall.SetActive(true);
                doorSet.SetWallState(wallState);
                break;
            case WallState.DOOR:
                wall.SetActive(false);
                doorSet.SetWallState(wallState);
                break;
        }
    }
}
