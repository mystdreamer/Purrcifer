using UnityEngine;

/// <summary>
/// Structure class representing a set of doors. 
/// </summary>
[System.Serializable]
public struct Wall
{
    [SerializeField] private GameObject _Wall;

    public void Enable()
    {
        _Wall.SetActive(true);
    }

    public void Disable()
    {
        _Wall.SetActive(false);
    }
}

/// <summary>
/// Structure class representing a set of doors. 
/// </summary>
[System.Serializable]
public struct DoorSet
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _wall;

    public void Enable()
    {
        _wall.SetActive(true);
        _door.SetActive(false);
    }

    public void Disable()
    {
        _wall.SetActive(false);
        _door.SetActive(false);
    }

    public void SetLockState(bool state)
    {
        _door.SetActive(state);
    }
}

/// <summary>
/// Structure class representing a set of doors. 
/// </summary>
[System.Serializable]
public struct HiddenRoomSet
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _breakable;

    public void Enable()
    {
        _wall.SetActive(true);
        _door.SetActive(false);
        _breakable.SetActive(true);
    }

    public void Disable()
    {
        _wall.SetActive(false);
        _door.SetActive(false);
        _breakable.SetActive(false);
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
    public Wall wallSet;
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
                wallSet.Enable();
                doorSet.Disable();
                break;
            case WallState.DOOR:
                wallSet.Disable();
                doorSet.Enable();
                break;
        }
    }
}
