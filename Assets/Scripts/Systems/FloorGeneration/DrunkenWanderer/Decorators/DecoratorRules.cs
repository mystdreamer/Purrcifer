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
        int startMarker = (int)MapIntMarkers.START;
        Vector2Int[] endpoints = plan.EndPoints;
        Vector2Int[] neighbors;

        foreach (Vector2Int endpoint in endpoints)
        {
            neighbors = plan.GetAdjacentCells(endpoint.x, endpoint.y);

            if(plan.GetMarkCount(neighbors, startMarker) == 0)
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