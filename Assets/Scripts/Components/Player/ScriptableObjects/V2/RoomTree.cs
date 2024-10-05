using UnityEngine;

[CreateAssetMenu(fileName = "New Room Data Tree SO", menuName = "Purrcifer/Rooms/RoomDataTreeSO")]
public class RoomTree : ScriptableObject
{
    public RoomDataSO[] startRooms;  
    public RoomDataSO[] normalRooms;  
    public RoomDataSO[] bossRooms;  
    public RoomDataSO[] treasureRooms;  
    public RoomDataSO[] hiddenRooms;  
}