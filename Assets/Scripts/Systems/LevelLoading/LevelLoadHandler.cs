using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    public enum LevelIDs
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

    public class LevelLoadHandler
    {
        public static void LoadLevel(int level)
        {
            SceneManager.LoadScene(level);
        }

        public static void LoadLevel(LevelIDs id)
        {
            SceneManager.LoadScene((int)id);
        }

    }
}