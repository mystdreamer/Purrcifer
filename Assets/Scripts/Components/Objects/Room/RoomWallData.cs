using Room.DoorData;
using UnityEngine;

namespace Room.DoorData
{

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

}

namespace Room.WallController
{

    /// <summary>
    /// Class controlling the type of wall assigned to a room. 
    /// </summary>
    public class RoomWallData : MonoBehaviour
    {
        [SerializeField] private WallType wallState = WallType.DOOR;

        /// <summary>
        /// The wall object. 
        /// </summary>
        public Wall wallSet;

        /// <summary>
        /// The set of door objects
        /// </summary>
        public DoorSet doorSet;
        public HiddenRoomSet hiddenRoomSet;

        /// <summary>
        /// Returns whether room set as a door. 
        /// </summary>
        public bool IsDoor => wallState == WallType.DOOR;

        /// <summary>
        /// The type of wall this is assigned to be. 
        /// </summary>
        public WallType WallType
        {
            get => wallState;
            set
            {
                this.wallState = value;

                switch (wallState)
                {
                    case WallType.WALL:
                        wallSet.Enable();
                        doorSet.Disable();
                        hiddenRoomSet.Disable();
                        break;
                    case WallType.DOOR:
                        wallSet.Disable();
                        doorSet.Enable();
                        hiddenRoomSet.Disable();
                        break;
                    case WallType.HIDDEN_ROOM:
                        wallSet.Disable();
                        doorSet.Disable();
                        hiddenRoomSet.Enable();
                        break;
                }
            }
        }

        public bool SetDoorLockState
        {
            set
            {
                if (wallState == WallType.DOOR)
                    doorSet.SetLockState(value);
                if (wallState == WallType.HIDDEN_ROOM)
                    hiddenRoomSet.SetLockState(value);
            }
        }
    }
}

