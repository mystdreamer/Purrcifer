using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FloorGeneration
{
    public static class Helpers_FloorPlan
    {
        /// <summary>
        /// Caches the endpoints within the level. 
        /// </summary>
        public static Vector2Int[] CacheEndpoints(FloorPlan plan)
        {
            Vector2Int[] matched = GetMarksWithType(plan, (int)MapIntMarkers.ROOM);
            return SortByAdjacency(plan, matched.ToList(), 1);
        }

        /// <summary>
        /// Returns a random normal room from the map. 
        /// </summary>
        /// <returns> Vector2Int with the map coords of the room. </returns>
        public static Vector2Int GetRandomRoom(FloorPlan plan)
        {
            Vector2Int[] _rooms = GetMarksWithType(plan, 1);
            return _rooms[UnityEngine.Random.Range(0, _rooms.Length - 1)];
        }

        /// <summary>
        /// Get a list of positions associated with the given mark. 
        /// </summary>
        /// <param name="mark"> The mark type to retrieve, </param>
        /// <returns> List of Vector2Int positions with the provided mark. </returns>
        public static Vector2Int[] GetMarksWithType(FloorPlan plan, int mark)
        {
            List<Vector2Int> matched = new List<Vector2Int>();

            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.plan.GetLength(1); j++)
                    if (plan[i, j] == mark) matched.Add(new Vector2Int(i, j));
            }
            return matched.ToArray();
        }

        /// <summary>
        /// Returns the number of rooms with a given mark. 
        /// </summary>
        /// <param name="points"> The array to check. </param>
        /// <param name="mark"> The mark to compare with. </param>
        public static int GetMarkCount(FloorPlan plan, Vector2Int[] points, int mark)
        {
            int count = 0;
            for (int i = 0; i < points.Length; i++)
                count += (plan[points[i]] != mark) ? 0 : 1;
            return count;
        }

        /// <summary>
        /// Sums the number of rooms adjacent to the cell. 
        /// </summary>
        /// <param name="plan"> The FloorPlan to evaluate. </param>
        /// <param name="x"> The x coord of the cell. </param>
        /// <param name="y"> The y coord of the cell. </param>
        /// <returns> Integer number representing the cells attached.</returns>
        public static int SumCellsAdjacent(FloorPlan plan, int x, int y)
        {
            Vector2Int[] neighbours = GetAdjacentCells(plan, x, y);
            int count = 0;

            foreach (Vector2Int n in neighbours)
            {
                count += (plan[n] != 0 && plan[n] != -1) ? 1 : 0;
            }
            return count;
        }

        /// <summary>
        /// Returns the positions of cells neighbouring the provided cell. 
        /// </summary>
        /// <param name="x"> The x position of the cell. </param>
        /// <param name="y"> The y position of the cell. </param>
        /// <returns> Vector2Int[] list containing the neighbouring addresses. </returns>
        public static Vector2Int[] GetAdjacentCells(FloorPlan plan, int x, int y)
        {
            return new Vector2Int[]
            {
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y),
            new Vector2Int(x, y + 1),
            new Vector2Int(x, y - 1)
            };
        }

        /// <summary>
        /// Returns a list of cells that match the required adjacency. 
        /// </summary>
        /// <param name="values"> The values to check. </param>
        /// <param name="adjCount"> The number of cells adjacent to the list provided. </param>
        /// <returns> List of cells matching the required adjacency count. </returns>
        public static Vector2Int[] SortByAdjacency(FloorPlan plan, List<Vector2Int> values, int adjCount)
        {
            List<Vector2Int> resulting = new List<Vector2Int>();
            for (int i = 0; i < values.Count; i++)
            {
                if (SumCellsAdjacent(plan, values[i].x, values[i].y) == adjCount)
                    resulting.Add(values[i]);
            }
            return resulting.ToArray();
        }

        /// <summary>
        /// Returns the total number of cells in the map. 
        /// </summary>
        public static int GetRoomCount(FloorPlan plan)
        {
            int count = 0;
            for (int i = 0; i < plan.plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.plan.GetLength(1); j++)
                {
                    if (plan[i, j] == 1) count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Static function for compiling the map into a string and printing to the console. 
        /// </summary>
        public static void Print2DArray<T>(T[,] matrix)
        {
            string outP = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    outP += (matrix[i, j] + "\t");
                }
                outP += "\n";
            }
            Debug.Log(outP);
        }

        /// <summary>
        /// Returns a list of cells that match the required adjacency. 
        /// </summary>
        /// <param name="values"> The values to check. </param>
        /// <param name="adjCount"> The number of cells adjacent to the list provided. </param>
        /// <returns> List of cells matching the required adjacency count. </returns>
        public static Vector2Int[] SortByAdjacency(FloorPlan plan, Vector2Int[] values, int adjCount)
        {
            List<Vector2Int> resulting = new List<Vector2Int>();
            for (int i = 0; i < values.Length; i++)
            {
                if (Helpers_FloorPlan.SumCellsAdjacent(plan, values[i].x, values[i].y) == adjCount)
                    resulting.Add(values[i]);
            }
            return resulting.ToArray();
        }

        public static bool MapSizeValid(FloorPlan plan, int min, int max)
        {
            int roomCount = GetRoomCount(plan);
            return (roomCount > min && roomCount < max);
        }
    }
}
