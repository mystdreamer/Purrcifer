using UnityEngine;
using ItemPool;

[CreateAssetMenu(fileName = "New Room Data SO", menuName = "Purrcifer/Rooms/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public string roomName;
    public int roomID = RandomIdGenerator.GetBase62(5);
    [Range(1, 100)]
    public int weighting = 0;
    public GameObject roomPrefab; 

    public static explicit operator Node<RoomDataSO>(RoomDataSO a)
    {
        return new Node<RoomDataSO>(a, a.roomID, a.weighting);
    }
}
