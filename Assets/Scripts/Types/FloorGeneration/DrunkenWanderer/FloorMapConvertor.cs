using JetBrains.Annotations;
using UnityEngine;

public enum DoorOpenOp
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public struct ObjectMap
{
    public GameObject[,] objectMap;
    public float roomSizeWidth;
    public float roomSizeHeight;

    public ObjectMap(FloorData data, FloorPlan plan)
    {
        int posValue;
        roomSizeWidth = data.roomWidth;
        roomSizeHeight = data.roomHeight;
        objectMap = new GameObject[data.floorWidth, data.floorHeight];
        for (int i = 0; i < plan.plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.plan.GetLength(1); j++)
            {
                posValue = plan.plan[i, j];

                switch (posValue)
                {
                    case (int)DecoratorMarkers.NONE:
                        break;
                    case (int)DecoratorMarkers.BOSS:
                        BuildBossRoom(i, j);
                        break;
                    case (int)DecoratorMarkers.START:
                        BuildStartRoom(i, j);
                        break;
                    case (int)DecoratorMarkers.ROOM:
                        BuildRoom(i, j);
                        break;
                    case (int)DecoratorMarkers.TREASURE:
                        BuildTreasureRoom(i, j);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void BuildBossRoom(int x, int y)
    {
        GameObject bRoomPrefab = MasterPool.Instance.BossPrefabTree.GetRandomPrefab(true);
        GameObject bossPrefab = MasterPool.Instance.BossRoomTree.GetRandomPrefab(false);

        GameObject bossRoom = GameObject.Instantiate(bRoomPrefab);
        GameObject boss = GameObject.Instantiate(bossPrefab);

        SetPosition(bossRoom, x, y);
        SetPosition(boss, x, y);
        boss.transform.parent = bossRoom.transform;
        objectMap[x, y] = bRoomPrefab;
    }

    private void BuildStartRoom(int x, int y)
    {
        GameObject startRoomPrefab = MasterPool.Instance.StartRoomTree.GetRandomPrefab(false);
        GameObject startRoom = GameObject.Instantiate(startRoomPrefab);

        //TODO: Change player spawn handling. 
        SetPosition(startRoom, x, y);
        SetToIndex(startRoom, x, y);
    }

    private void BuildRoom(int x, int y)
    {
        GameObject roomPrefab = MasterPool.Instance.NormalRoomTree.GetRandomPrefab(true);
        GameObject roomInst = GameObject.Instantiate(roomPrefab);
        SetPosition(roomInst, x, y);
        SetToIndex(roomInst, x, y);
    }

    private void BuildTreasureRoom(int x, int y)
    {
        GameObject treasureRoomPrefab = MasterPool.Instance.TreasureRoomTree.GetRandomPrefab(false);
        GameObject tRoomInst = GameObject.Instantiate(treasureRoomPrefab);
        //TODO: Setup handling randomising item drops and placing in the room. 
        SetPosition(tRoomInst, x, y);
        SetToIndex(tRoomInst, x, y);
    }

    public void SetPosition(GameObject gameObject, int x, int y) =>
        gameObject.transform.position = new Vector2(x * roomSizeWidth, y * roomSizeHeight);

    public void SetToIndex(GameObject prefab, int x, int y) =>
        objectMap[x, y] = prefab;

    public void EnableDoors(DoorOpenOp[] ops, int x, int y)
    {
        foreach (DoorOpenOp operation in ops)
            EnableDoors(operation, x, y);
    }

    public void EnableDoors(DoorOpenOp op, int x, int y)
    {
        //TODO: Implement this. 
        //Steps: 
        // if(up) ->
        // Check if (up exists) [x, y -1]
        //  -- Activate neighbours down door. 
        //  -- Activate up door. 

        // if(down) ->
        // Check if (down exists) [x, y + 1]
        //  -- Activate neighbours up door. 
        //  -- Activate down door.

        // if(left) ->
        // Check if (left exists) [x - 1, y]
        //  -- Activate neighbours right door. 
        //  -- Activate left door.

        // if(right) ->
        // Check if (up exists) [x + 1, y]
        //  -- Activate neighbours down door. 
        //  -- Activate up door. 
    }

    #region Convenience functions. 
    public void EnableDoors(DoorOpenOp op, Vector2Int Position) =>
        EnableDoors(op, Position.x, Position.y);

    public void EnableDoors(DoorOpenOp[] ops, Vector2Int Position) =>
        EnableDoors(ops, Position.x, Position.y);

    public void SetToIndex(GameObject prefab, Vector2Int position) =>
        SetToIndex(prefab, position.x, position.y);
    #endregion
}

public class FloorMapConvertor
{

}
