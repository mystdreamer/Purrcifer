namespace Purrcifer.FloorGeneration.RoomResolution
{

    /// <summary>
    /// Used to resolve room rulesets. 
    /// </summary>
    public static class DoorTypeResolution
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
        public static void ResolveRoomDoors(ObjectMap map, int x, int y)
        {
            if (map[x, y] == null)
                return;

            //Get controller. 
            RoomController controller = map[x, y].GetComponent<RoomController>();
            //Cache marker type
            MapIntMarkers mType = controller.roomType;

            //Get neighbours. 
            AttemptNeighbourResolution(map, controller, x + 1, y, WallDirection.RIGHT, WallDirection.LEFT);
            AttemptNeighbourResolution(map, controller, x - 1, y, WallDirection.LEFT, WallDirection.RIGHT);
            AttemptNeighbourResolution(map, controller, x, y + 1, WallDirection.UP, WallDirection.DOWN);
            AttemptNeighbourResolution(map, controller, x, y - 1, WallDirection.DOWN, WallDirection.UP);
        }

        private static void AttemptNeighbourResolution(
            ObjectMap map, RoomController room, int x, int y,
            WallDirection AOp, WallDirection BOp)
        {
            if (map[x, y] == null)
                return;

            //Get neighbour controller. 
            RoomController nController = map[x, y].GetComponent<RoomController>();

            if (nController.roomType == MapIntMarkers.NONE)
                return;
            else
                ResolvePriority(room, nController, AOp, BOp);
        }

        private static void ResolvePriority(
            RoomController controllerA, RoomController controllerB,
            WallDirection directionA, WallDirection directionB)
        {
            WallType a = WallType.WALL;
            WallType b = WallType.WALL;
            MapIntMarkers aType = controllerA.roomType;
            MapIntMarkers bType = controllerB.roomType;

            //Check if this is a room -> type interaction.
            if (RULE_CONTAINS(aType, bType, MapIntMarkers.ROOM))
            {
                //If is room/room enable door.
                if (RULE_AB_INTERSECT(aType, bType, MapIntMarkers.ROOM))
                    a = b = WallType.DOOR;

                //If is room/[Start, Treasure, Boss] with special -> enable door.
                if (RULE_INTERSECTION(aType, bType,
                    new MapIntMarkers[] { MapIntMarkers.START, MapIntMarkers.TREASURE, MapIntMarkers.BOSS }))
                    a = b = WallType.DOOR;
                //If is room/Hidden -> enable hidden door.
                if (RULE_CONTAINS(aType, bType, MapIntMarkers.HIDDEN_ROOM))
                    a = b = WallType.HIDDEN_ROOM;
            }

            //Check remaining hidden room interactions.
            if (RULE_CONTAINS(aType, bType, MapIntMarkers.HIDDEN_ROOM))
            {
                if (RULE_CONTAINS(aType, bType, MapIntMarkers.START) || RULE_CONTAINS(aType, bType, MapIntMarkers.TREASURE))
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
