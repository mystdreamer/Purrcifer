using ItemPool;
using UnityEngine;

/// <summary>
/// Component script holding references to all item trees, acts as a hub for interfacing. 
/// </summary>
public class MasterTree : MonoBehaviour
{
    /// <summary>
    /// The singleton reference to the MasterPool. 
    /// </summary>
    private static MasterTree _instance;
    public ItemDataSOTree itemTreeSO;
    public RoomTreeSO roomTreeSO;

    public ItemTree itemTree;
    public RoomTree roomTree;

    public static MasterTree Instance => _instance;

    public GameObject GetItemSpawnTreasureRoom =>
        Instance.itemTree.poolTree.GetRandomPrefab(false);

    public ItemDataSO GetItemSpawnTreasureRoomSO =>
        Instance.itemTree.poolTree.GetNode(false);

    public static GameObject GetStartRoomPrefab =>
        Instance.roomTree.startRoomPool.GetRandomPrefab(false);

    public static GameObject GetNormalRoomPrefab =>
        Instance.roomTree.normalRoomPool.GetRandomPrefab(false);

    public static GameObject GetTreasureRoomPrefab =>
        Instance.roomTree.treasureRoomPool.GetRandomPrefab(false);

    public static GameObject GetBossRoomPrefab =>
        Instance.roomTree.bossRoomPool.GetRandomPrefab(false);

    public static GameObject GetHiddenRoomPrefab =>
        Instance.roomTree.hiddenRoomPool.GetRandomPrefab(false);

    private void Awake()
    {
        //Generate singleton. 
        if (_instance == null)
        {
            _instance = this;
        }

        itemTree = new ItemTree(itemTreeSO);
        roomTree = new RoomTree(roomTreeSO);
    }
}