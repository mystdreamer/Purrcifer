[System.Serializable]
public class FloorData
{
    public int roomCountMin;
    public int roomCountMax;
    public int floorWidth;
    public int floorHeight;
    public int initialX, initialY;
    public Range rangeProbX;
    public float spawnOver;
    public float roomWidth;
    public float roomHeight;

    public bool SpawnChance => UnityEngine.Random.Range(rangeProbX.min, rangeProbX.max) > spawnOver;
}
