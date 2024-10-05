using UnityEngine;

[CreateAssetMenu(fileName = "New Room Data SO", menuName = "Purrcifer/Rooms/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public string roomName;
    public int roomID = RandomIdGenerator.GetBase62(5);
    public GameObject roomPrefab; 
}
