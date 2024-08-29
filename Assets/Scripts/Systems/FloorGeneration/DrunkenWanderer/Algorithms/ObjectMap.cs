using JetBrains.Annotations;
using UnityEngine;

public enum DoorOpenOp
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class ObjectMap
{
    public GameObject[,] objectMap;
    public float roomSizeWidth;
    public float roomSizeHeight;
    public Vector3 initalPosition;
    public Vector2Int bossRoomPos; 
    public Vector2Int startRoomPos; 
    public Vector2Int treasureRoomPos; 

    public ObjectMap(FloorData data, FloorPlan plan)
    {
        initalPosition = new Vector3(data.initialX, data.initialY);
        roomSizeWidth = data.roomWidth;
        roomSizeHeight = data.roomHeight;
        objectMap = new GameObject[plan.plan.GetLength(0), plan.plan.GetLength(1)];
    }

    public void GenerateObject(int value, int x, int y)
    {
        switch (value)
        {
            case (int)MapIntMarkers.NONE:
                break;
            case (int)MapIntMarkers.BOSS:
                BuildBossRoom(x, y);
                break;
            case (int)MapIntMarkers.START:
                BuildStartRoom(x, y);
                break;
            case (int)MapIntMarkers.ROOM:
                BuildRoom(x, y);
                break;
            case (int)MapIntMarkers.TREASURE:
                BuildTreasureRoom(x, y);
                break;
            default:
                break;
        }
    }

    private void BuildBossRoom(int x, int y)
    {
        GameObject bRoomPrefab = MasterTree.Instance.BossRoomTree.GetRandomPrefab(false);
        GameObject bossRoom = GameObject.Instantiate(bRoomPrefab);
        bossRoomPos = new Vector2Int(x, y);
        SetPosition(bossRoom, x, y);
        SetToIndex(bossRoom, x, y);
    }

    private void BuildStartRoom(int x, int y)
    {
        GameObject startRoomPrefab = MasterTree.Instance.StartRoomTree.GetRandomPrefab(false);
        GameObject startRoom = GameObject.Instantiate(startRoomPrefab);
        startRoomPos = new Vector2Int(x, y);
        //TODO: Change player spawn handling. 
        SetPosition(startRoom, x, y);
        SetToIndex(startRoom, x, y);
    }

    private void BuildRoom(int x, int y)
    {
        GameObject roomPrefab = MasterTree.Instance.NormalRoomTree.GetRandomPrefab(false);
        GameObject roomInst = GameObject.Instantiate(roomPrefab);
        SetPosition(roomInst, x, y);
        SetToIndex(roomInst, x, y);
    }

    private void BuildTreasureRoom(int x, int y)
    {
        GameObject treasureRoomPrefab = MasterTree.Instance.TreasureRoomTree.GetRandomPrefab(false);
        GameObject tRoomInst = GameObject.Instantiate(treasureRoomPrefab);
        //TODO: Setup handling randomising item drops and placing in the room. 
        treasureRoomPos = new Vector2Int(x, y);
        SetPosition(tRoomInst, x, y);
        SetToIndex(tRoomInst, x, y);
    }

    public void SetPosition(GameObject gameObject, int x, int y) =>
        gameObject.transform.position = new Vector3(initalPosition.x + x * roomSizeWidth, 0, initalPosition.y - y * roomSizeHeight);

    public void SetToIndex(GameObject prefab, int x, int y) =>
        objectMap[x, y] = prefab;

    /// <summary>
    /// Get an object from within the object map. 
    /// </summary>
    /// <param name="x"> The map x coord of the item. </param>
    /// <param name="y"> The map y coord of the object. </param>
    /// <returns></returns>
    private GameObject GetObject(int x, int y)
    {
        if (x < objectMap.GetLength(0) && x >= 0 &&
            y < objectMap.GetLength(1) && y >= 0)
            return objectMap[x, y];
        return null;
    }

    /// <summary>
    /// Enables doors around a give cell. 
    /// </summary>
    /// <param name="x"> The x coord of the cell. </param>
    /// <param name="y"> The y coord of the cell. </param>
    public void EnableDoors(int x, int y)
    {
        //Cache neighbour marks.
        GameObject _room = GetObject(x, y);
        GameObject _obj;

        if (_room == null)
            return;
        RoomDoorController _roomCTRLR = _room.GetComponent<RoomDoorController>();

        if (_roomCTRLR == null)
            return;

        _obj = GetObject(x + 1, y);

        if (_obj != null)
        {
            _obj.GetComponent<RoomDoorController>().SetDoorState(DoorOpenOp.LEFT);
            _roomCTRLR.SetDoorState(DoorOpenOp.RIGHT);
        }

        _obj = GetObject(x - 1, y);

        if (_obj != null)
        {
            _obj.GetComponent<RoomDoorController>().SetDoorState(DoorOpenOp.RIGHT);
            _roomCTRLR.SetDoorState(DoorOpenOp.LEFT);
        }

        _obj = GetObject(x, y - 1);

        if (_obj != null)
        {
            _obj.GetComponent<RoomDoorController>().SetDoorState(DoorOpenOp.DOWN);
            _roomCTRLR.SetDoorState(DoorOpenOp.UP);
        }

        _obj = GetObject(x, y + 1);

        if (_obj != null)
        {
            _obj.GetComponent<RoomDoorController>().SetDoorState(DoorOpenOp.UP);
            _roomCTRLR.SetDoorState(DoorOpenOp.DOWN);
        }
    }
}