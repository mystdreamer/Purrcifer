namespace Purrcifer.FloorGeneration.RoomResolution
{
    public static class FloorMapConversion
    {
        public static void GenerateObjectMap(FloorPlan plan, ref ObjectMap map)
        {
            int posValue;

            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for(int j = 0; j < plan.plan.GetLength(1); j++)
                {
                    posValue = plan.plan[i, j];
                    map.GenerateObject(plan, posValue, i, j);
                }
            }
        }

        public static void OpenDoors(FloorPlan plan, ref ObjectMap map)
        {
            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.plan.GetLength(1); j++)
                {
                    map.EnableDoors(i, j);
                }
            }
        }
    }
}
