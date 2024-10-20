using System.Collections.Generic;
using UnityEngine;

namespace Purrcifer.FloorGeneration.RoomResolution
{
    /// <summary>
    /// Resolves the spawning of room objects within the scene. 
    /// </summary>
    public static class RoomInstancing
    {
        public static class RoomMappingConversions
        {
            public static Dictionary<MapIntMarkers, WallType> map =
                new Dictionary<MapIntMarkers, WallType>() {
            { MapIntMarkers.NONE, WallType.NONE },
            { MapIntMarkers.ROOM, WallType.WALL },
            { MapIntMarkers.START, WallType.WALL },
            { MapIntMarkers.BOSS, WallType.WALL },
            { MapIntMarkers.TREASURE, WallType.WALL },
            { MapIntMarkers.HIDDEN_ROOM, WallType.HIDDEN_ROOM },
                };
        }

        public static GameObject BuildRoomObject(MapIntMarkers type, Vector3 position)
        {
            GameObject prefab = GetObjectRef(type);

            if (prefab == null)
                return null;

            GameObject obj = GameObject.Instantiate(prefab);

            WallType wallType = RoomMappingConversions.map[type];
            RoomController c = obj.GetComponent<RoomController>();
            c.MarkerType = wallType;
            c.roomType = type;
                obj.name = obj.name + "[" + position.x + ", 0, " + position.z + "]";

            //Set the position of the object in world space. 
            obj.transform.position = position;

            return obj;
        }

        /// <summary>
        /// Generates an object within the map. 
        /// </summary>
        /// <param name="marker"> The marker type representing the object. </param>
        /// <param name="x"> The x coordinate of the object. </param>
        /// <param name="y"> The y coordinate of the object. </param>
        public static GameObject GetObjectRef(MapIntMarkers marker)
        {
            switch (marker)
            {
                case MapIntMarkers.BOSS:
                    return MasterTree.GetBossRoomPrefab;
                case MapIntMarkers.START:
                    return MasterTree.GetStartRoomPrefab;
                case MapIntMarkers.ROOM:
                    return MasterTree.GetNormalRoomPrefab;
                case MapIntMarkers.TREASURE:
                    return MasterTree.GetTreasureRoomPrefab;
                case MapIntMarkers.HIDDEN_ROOM:
                    return MasterTree.GetHiddenRoomPrefab;
            }
            return null;
        }
    }
}
