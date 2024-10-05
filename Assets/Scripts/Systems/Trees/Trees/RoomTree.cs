using ItemPool;

[System.Serializable]
public class RoomTree
{
    /// <summary>
    /// The Scriptable object used to define the pools contents. 
    /// </summary>
    public RoomTreeSO pool;

    /// <summary>
    /// The pools tree.
    /// </summary>
    public RoomBBST startRoomPool;
    public RoomBBST normalRoomPool;
    public RoomBBST treasureRoomPool;
    public RoomBBST bossRoomPool;
    public RoomBBST hiddenRoomPool;

    public RoomTree(RoomTreeSO pool)
    {
        this.pool = pool;
        this.treasureRoomPool = new RoomBBST(pool.treasureRooms);
        this.normalRoomPool = new RoomBBST(pool.normalRooms);
        this.startRoomPool = new RoomBBST(pool.startRooms);
        this.hiddenRoomPool = new RoomBBST(pool.hiddenRooms);
        this.bossRoomPool = new RoomBBST(pool.bossRooms); 
    }
}