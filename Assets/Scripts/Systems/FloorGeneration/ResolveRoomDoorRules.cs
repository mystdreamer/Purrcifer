using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.UIElements;
using Room.DoorData;

namespace Purrcifer.FloorGeneration.RoomResolution
{
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

        public static GameObject BuildRoomObject(
            MapIntMarkers type,
            int initalX, int initalY,
            int xStep, int yStep,
            float width, float height)
        {
            GameObject prefab = GetObjectRef(type);

            if (prefab == null)
                return null;

            GameObject obj = GameObject.Instantiate(prefab);

            WallType wallType = RoomMappingConversions.map[type];
            obj.GetComponent<RoomController>().MarkerType = wallType;
            obj.name = obj.name + "[" + xStep + ", " + yStep + "]";

            //Set the position of the object in world space. 
            obj.transform.position =
                new UnityEngine.Vector3(initalX + (xStep * width), 0, initalY - yStep * height);

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
                case MapIntMarkers.NONE:
                    return null;
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
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Used to resolve room rulesets. 
    /// </summary>
    public static class ResolveRoomDoorRules
    {
            //Room rules:  
            //[Anything -> None] = Wall.
            //[Start->Boss] -> Wall.
            //[Start->Treasure] -> Wall.
            //[Start->Boss] -> Wall.
            //[Hidden->Normal, Treasure, Start] -> Hidden wall.
            //[Hidden -> Boss] -> Wall.
            
            //          Start | Hidden | Treasure | Boss | Normal
            //Start     |  	--|       O|         X|     X|      O
            //Hidden    |  	 O|      --|         O|     X|      O
            //Treasure  |  	 X|       O|        --|     X|      O
            //Boss      |  	 X|       X|         X|    --|      O
            //Normal    |  	 O|       O|         O|     O|      O
            
            //Normal(O): S, H, T, B N
            //Boss(O): N
            //Start(O): H, N
            //Hidden(O): S, T, N
            //Treasure(O): H, N

        /// <summary>
        /// Resolves linking rooms based on adjacency. 
        /// </summary>
        /// <param name="floorPlan"> The floor plan to use for generation. </param>
        /// <param name="map"> The object map to work on. </param>
        /// <param name="x"> The current x index of the room. </param>
        /// <param name="y"> The current y index of the room. </param>
        public static void ResolveRoomDoors(FloorPlan floorPlan, ObjectMap map, int x, int y)
        {
            if (map[x, y] == null)
                return;

            //Get controller. 
            RoomController controller = map[x, y].GetComponent<RoomController>();
            //Cache marker type
            MapIntMarkers mType = (MapIntMarkers)floorPlan[x, y];

            //Get neighbours. 
            AttemptNeighbourResolution(floorPlan, map, controller, mType, x + 1, y, WallDirection.RIGHT, WallDirection.LEFT);
            AttemptNeighbourResolution(floorPlan, map, controller, mType, x - 1, y, WallDirection.LEFT, WallDirection.RIGHT);
            AttemptNeighbourResolution(floorPlan, map, controller, mType, x, y - 1, WallDirection.UP, WallDirection.DOWN);
            AttemptNeighbourResolution(floorPlan, map, controller, mType, x, y + 1, WallDirection.DOWN, WallDirection.UP);
        }

        private static void AttemptNeighbourResolution(
            FloorPlan floorplan, ObjectMap map, RoomController room,
            MapIntMarkers roomType, int x, int y,
            WallDirection AOp, WallDirection BOp)
        {
            if (map[x, y] == null)
                return;

            //Get neighbour controller. 
            RoomController controller = map[x, y].GetComponent<RoomController>();
            MapIntMarkers neighbourType = (MapIntMarkers)floorplan[x, y];


            if (neighbourType == MapIntMarkers.NONE)
                return;
            else
                ResolvePriority(room, controller, roomType, neighbourType, AOp, BOp);
        }

        private static void ResolvePriority(
            RoomController controllerA, RoomController controllerB,
            MapIntMarkers markerA, MapIntMarkers markerB,
            WallDirection directionA, WallDirection directionB)
        {
            WallType a = WallType.WALL;
            WallType b = WallType.WALL;

            //Check if this is a room -> type interaction.
            if (RULE_CONTAINS(markerA, markerB, MapIntMarkers.ROOM))
            {
                //If is room/room enable door.
                if (RULE_AB_INTERSECT(markerA, markerB, MapIntMarkers.ROOM))
                    a = b = WallType.DOOR;

                //If is room/[Start, Treasure, Boss] with special -> enable door.
                if (RULE_INTERSECTION(markerA, markerB,
                    new MapIntMarkers[] { MapIntMarkers.START, MapIntMarkers.TREASURE, MapIntMarkers.BOSS }))
                    a = b = WallType.DOOR;
                //If is room/Hidden -> enable hidden door.
                if (RULE_CONTAINS(markerA, markerB, MapIntMarkers.HIDDEN_ROOM))
                    a = b = WallType.HIDDEN_ROOM;
            }

            //Check remaining hidden room interactions.
            if (RULE_CONTAINS(markerA, markerB, MapIntMarkers.HIDDEN_ROOM))
            {
                if (RULE_CONTAINS(markerA, markerB, MapIntMarkers.START) || RULE_CONTAINS(markerA, markerB, MapIntMarkers.TREASURE))
                    a = b = WallType.HIDDEN_ROOM;
            }

            controllerA.SetRoomState(directionA, a);
            controllerB.SetRoomState(directionB, b);
        }

        private static bool RULE_AND(MapIntMarkers A, MapIntMarkers B, MapIntMarkers checkA, MapIntMarkers checkB)
        {
            return (RULE_CONTAINS(A, B, checkA) && RULE_CONTAINS(A, B, checkB));
        }

        private static bool RULE_INTERSECTION(MapIntMarkers A, MapIntMarkers B, MapIntMarkers[] checks)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                if (A == checks[i] | B == checks[i])
                    return true;
            }
            return false;
        }

        private static bool RULE_AB_INTERSECT(MapIntMarkers typeA, MapIntMarkers typeB, MapIntMarkers checkType)
        {
            return (typeA == checkType | typeB == checkType);
        }

        private static bool RULE_CONTAINS(MapIntMarkers A, MapIntMarkers B, MapIntMarkers type)
        {
            return ((A == type | B == type));
        }
    }
}
