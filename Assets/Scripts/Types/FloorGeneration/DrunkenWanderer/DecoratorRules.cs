using UnityEngine;

public static class DecoratorRules
{

    public static void StartDecorator(ref FloorPlan plan, out bool success)
    {
        plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)DecoratorMarkers.START;
        success = true;
    }

    public static void BossDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;

        foreach (Vector2Int endpoint in endpoints)
        {
            if (plan.GetRoomState(endpoint.x + 1, endpoint.y) != (int)DecoratorMarkers.START &&
                plan.GetRoomState(endpoint.x - 1, endpoint.y) != (int)DecoratorMarkers.START &&
                plan.GetRoomState(endpoint.x, endpoint.y + 1) != (int)DecoratorMarkers.START &&
                plan.GetRoomState(endpoint.x, endpoint.y - 1) != (int)DecoratorMarkers.START)
            {

                plan.ChangeMark(endpoint, (int)DecoratorMarkers.BOSS);
                break;
            }
        }
        plan.CacheEndpoints();
        success = true;
    }

    public static void TreasureDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;
        plan.ChangeMark(endpoints[0], (int)DecoratorMarkers.TREASURE);
        plan.CacheEndpoints();
        success = true;
    }
}