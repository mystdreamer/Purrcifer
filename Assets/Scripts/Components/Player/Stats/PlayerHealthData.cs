using UnityEngine;

namespace Purrcifer.PlayerData
{
    /// <summary>
    /// Serializable data class containing health data. 
    /// </summary>
    [System.Serializable]
    public class PlayerHealthData
    {
        /// <summary>
        /// The minimum range of the pool.
        /// </summary>
        [Header("The minimum health.")]
        public int min;

        /// <summary>
        /// The maximum range of the pool.
        /// </summary>
        [Header("The maximum health.")]
        public int max;

        /// <summary>
        /// The maximum range of the pool.
        /// </summary>
        [Header("The current health.")]
        public int current;

        /// <summary>
        /// CTOR. 
        /// </summary>
        /// <param name="min"> The minimum health value of the player. </param>
        /// <param name="max"> The maximum health value of the player. </param>
        /// <param name="current"> The current health of the player. </param>
        public PlayerHealthData(int min, int max, int current)
        {
            this.min = min;
            this.max = max;
            this.current = current;
        }
    }
}
