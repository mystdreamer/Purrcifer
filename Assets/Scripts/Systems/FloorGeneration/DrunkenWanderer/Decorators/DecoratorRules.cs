using UnityEngine;

/// <summary>
/// Class containing rules for building maps. 
/// </summary>
public static class DecoratorRules
{
    /// <summary>
    /// Sets the marker for the start room. 
    /// </summary>
    /// <param name="plan"> The floor plan to mark. </param>
    /// <param name="success"> Returns true if the operation was successful</param>
    public static void StartDecorator(ref FloorPlan plan, out bool success)
    {
        plan.plan[plan.floorCenter.x, plan.floorCenter.y] = (int)MapIntMarkers.START;
        success = true;
    }

    /// <summary>
    /// Sets the marker for the boss room. 
    /// </summary>
    /// <param name="plan"> The floor plan to mark. </param>
    /// <param name="success"> Returns true if the operation was successful</param>
    public static void BossDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;

        foreach (Vector2Int endpoint in endpoints)
        {
            if (plan[endpoint.x + 1, endpoint.y] != (int)MapIntMarkers.START &&
                plan[endpoint.x - 1, endpoint.y] != (int)MapIntMarkers.START &&
                plan[endpoint.x, endpoint.y + 1] != (int)MapIntMarkers.START &&
                plan[endpoint.x, endpoint.y - 1] != (int)MapIntMarkers.START)
            {

                plan[endpoint.x, endpoint.y] = (int)MapIntMarkers.BOSS;
                break;
            }
        }
        plan.CacheEndpoints();
        success = true;
    }

    /// <summary>
    /// Sets the marker for the treasure room. 
    /// </summary>
    /// <param name="plan"> The floor plan to mark. </param>
    /// <param name="success"> Returns true if the operation was successful</param>
    public static void TreasureDecorator(ref FloorPlan plan, out bool success)
    {
        Vector2Int[] endpoints = plan.EndPoints;
        plan[endpoints[0].x, endpoints[0].y] = (int)MapIntMarkers.TREASURE;
        plan.CacheEndpoints();
        success = true;
    }
}