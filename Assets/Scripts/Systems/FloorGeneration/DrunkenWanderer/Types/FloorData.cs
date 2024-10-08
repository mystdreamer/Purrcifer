﻿
/// <summary>
/// Data class used to generate FloorMaps. 
/// </summary>
[System.Serializable]
public class FloorData
{
    public int roomCountMin;
    public int roomCountMax;
    public int floorWidth;
    public int floorHeight;
    public int initialX, initialY;
    public float spawnOver;
    public int numberOfEndpoints = 3;
}
