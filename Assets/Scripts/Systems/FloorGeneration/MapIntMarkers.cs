using System.Collections.Generic;

/// <summary>
/// Enums used during the generation of int mappings. 
/// </summary>
public enum MapIntMarkers
{
    NONE = 0,
    ROOM = 1,
    START = 2,
    BOSS = 3,
    TREASURE = 4, 
    HIDDEN_ROOM = 5
}

public enum WallType : int
{
    NONE = 0,
    WALL = 1,
    DOOR = 2,
    HIDDEN_ROOM = 3
}
