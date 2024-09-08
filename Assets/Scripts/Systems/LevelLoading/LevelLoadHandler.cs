using UnityEngine.SceneManagement;

namespace Purrcifer.LevelLoading
{

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