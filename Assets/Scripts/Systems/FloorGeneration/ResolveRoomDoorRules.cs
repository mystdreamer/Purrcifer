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

namespace Purrcifer.FloorGeneration.RoomResolution
{
    public static class RoomInstancing
    {
        public static GameObject BuildRoomObject(
            MapIntMarkers type, 
            int initalX, int initalY, 
            int xStep, int yStep, 
            float width, float height)
        {
            GameObject prefab = ObjectGenHelper.GetObjectRef(type);

            if (prefab == null)
                return null;
            
            GameObject obj = GameObject.Instantiate(prefab);

            WallType wallType = ObjectGenHelper.RoomMappingConversions.map[type];
            obj.GetComponent<RoomController>().MarkerType = wallType;
            obj.name = obj.name + "[" + xStep + ", " + yStep + "]";

            //Set the position of the object in world space. 
            obj.transform.position =
                new UnityEngine.Vector3(initalX + (xStep * width), 0, initalY - yStep * height);

            return obj;
        }
    }

    /// <summary>
    /// Used to resolve room rulesets. 
    /// </summary>
    public static class ResolveRoomDoorRules
    {

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
            RoomController roomACont, RoomController roomBCont,
            MapIntMarkers roomA, MapIntMarkers roomB,
            WallDirection AOp, WallDirection BOp)
        {
            switch (roomA)
            {
                case MapIntMarkers.NONE:
                    // this is a void and thus should not be resolved.
                    break;
                case MapIntMarkers.ROOM:
                    if (roomA == MapIntMarkers.ROOM && roomB == MapIntMarkers.ROOM)
                    {
                        roomACont.SetRoomState(AOp, WallType.DOOR);
                        roomBCont.SetRoomState(BOp, WallType.DOOR);
                    }
                    break;
                case MapIntMarkers.START:
                    if (RoomsContain(roomA, roomB, MapIntMarkers.ROOM))
                    {
                        roomACont.SetRoomState(AOp, WallType.DOOR);
                        roomBCont.SetRoomState(BOp, WallType.DOOR);
                    }
                    break;
                case MapIntMarkers.BOSS:
                    if (RoomsContain(roomA, roomB, MapIntMarkers.ROOM))
                    {
                        roomACont.SetRoomState(AOp, WallType.DOOR);
                        roomBCont.SetRoomState(BOp, WallType.DOOR);
                    }
                    break;
                case MapIntMarkers.TREASURE:
                    if (RoomsContain(roomA, roomB, MapIntMarkers.ROOM))
                    {
                        roomACont.SetRoomState(AOp, WallType.DOOR);
                        roomBCont.SetRoomState(BOp, WallType.DOOR);
                    }
                    break;
                case MapIntMarkers.HIDDEN_ROOM:
                    if (RoomsContain(roomA, roomB, MapIntMarkers.ROOM))
                    {
                        roomACont.SetRoomState(AOp, WallType.HIDDEN_ROOM);
                        roomBCont.SetRoomState(BOp, WallType.HIDDEN_ROOM);
                    }
                    break;
                default:
                    // this is a void and thus should not be resolved.
                    break;
            }
        }

        private static bool RoomsContain(MapIntMarkers typeA, MapIntMarkers typeB, MapIntMarkers checkType) {
            return (typeA == checkType | typeB == checkType);
        }

    }
}
