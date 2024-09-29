using UnityEngine;

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
    public WallType state;

    /// <summary>
    /// Boolean denoting if the wall is a door. 
    /// </summary>
    public bool isDoor;

    /// <summary>
    /// Changes the state of the wall. 
    /// </summary>
    /// <param name="state"></param>
    public void SetState(WallType state)
    {
        switch (state)
        {
            case WallType.DOOR:
                doorWall.SetActive(true);
                door.SetActive(false);
                wall.SetActive(false);
                this.state = state;
                isDoor = true;
                break;

            case WallType.WALL:
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
