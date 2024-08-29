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

            if (plan.GetMarkCount(neighbors, startMarker) == 0
                && plan[endpoint] != startMarker)
            {
                plan[endpoint.x, endpoint.y] = (int)MapIntMarkers.BOSS;
                plan.CacheEndpoints();
                success = true;
                return;
            }
        }
        success = false;

    }

    /// <summary>
    /// Sets the marker for the treasure room. 
    /// </summary>
    /// <param name="plan"> The floor plan to mark. </param>
    /// <param name="success"> Returns true if the operation was successful</param>
    public static void TreasureDecorator(ref FloorPlan plan, out bool success)
    {
        int startMarker = (int)MapIntMarkers.START;
        int bossMarker = (int)MapIntMarkers.BOSS;
        Vector2Int[] endpoints = plan.EndPoints;
        Vector2Int[] neighbors;

        foreach (Vector2Int endpoint in endpoints)
        {
            neighbors = plan.GetAdjacentCells(endpoint.x, endpoint.y);

            if (plan.GetMarkCount(neighbors, bossMarker) == 0 &&
                plan[endpoint] != startMarker &&
                plan[endpoint] != bossMarker)
            {
                plan[endpoint.x, endpoint.y] = (int)MapIntMarkers.TREASURE;
                success = true;
                plan.CacheEndpoints();
                return;
            }
        }
        success = false;

    }
}