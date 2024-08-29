using UnityEngine;

public static class DecoratorRules
{

    public static void StartDecorator(ref FloorPlan plan, out bool success)
    {
        plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)MapIntMarkers.START;
        success = true;
    }

    public static void BossDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;

        foreach (Vector2Int endpoint in endpoints)
        {
            if (plan.GetRoomState(endpoint.x + 1, endpoint.y) != (int)MapIntMarkers.START &&
                plan.GetRoomState(endpoint.x - 1, endpoint.y) != (int)MapIntMarkers.START &&
                plan.GetRoomState(endpoint.x, endpoint.y + 1) != (int)MapIntMarkers.START &&
                plan.GetRoomState(endpoint.x, endpoint.y - 1) != (int)MapIntMarkers.START)
            {

                plan.ChangeMark(endpoint, (int)MapIntMarkers.BOSS);
                break;
            }
        }
        plan.CacheEndpoints();
        success = true;
    }

    public static void TreasureDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;
        plan.ChangeMark(endpoints[0], (int)MapIntMarkers.TREASURE);
        plan.CacheEndpoints();
        success = true;
    }
}