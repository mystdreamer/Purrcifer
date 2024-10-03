#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Manager : MonoBehaviour
{
    // Singleton instance to ensure only one instance of Debug_Manager exists
    private static Debug_Manager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // If F5 is pressed return to splash screen
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("F5 pressed: Loading SplashScreen");
            SceneManager.LoadScene("SplashScreen");
        }

        // If F6 is pressed load test scene
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("F6 pressed: Loading test level generation scene");
            SceneManager.LoadScene("TESTLEVELS");
        }
    }
}
#endif
