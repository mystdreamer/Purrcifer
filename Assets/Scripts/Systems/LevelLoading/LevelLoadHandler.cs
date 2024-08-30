using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    /// <summary>
    /// The level IDs included in the project. 
    /// </summary>
    public enum LevelID
    {
        SPLASH = 0,
        MAIN = 1,
        LEVEL_1 = 2,
        LEVEL_2 = 3,
        LEVEL_3 = 4,
        LEVEL_4 = 5,
        LEVEL_5 = 6,
        END_GAME = 7,
        CREDITS = 8
    }

    /// <summary>
    /// Class handling loading levels at runtime. 
    /// </summary>
    public class LevelLoadHandler
    {
        /// <summary>
        /// Load the given level int. 
        /// </summary>
        /// <param name="level"> The build index to load. </param>
        public static void LoadLevel(int level)
        {
            SceneManager.LoadScene(level);
        }

        /// <summary>
        /// Load a given level using a LevelID. 
        /// </summary>
        /// <param name="id"> The LevelID to load. </param>
        public static void LoadLevel(LevelID id)
        {
            SceneManager.LoadScene((int)id);
        }
    }
}