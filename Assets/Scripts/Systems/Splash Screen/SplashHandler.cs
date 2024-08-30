using System.Collections;
using UnityEngine;

public class SplashHandler : MonoBehaviour
{
    public float waitTime = 0;
    private SplashContentLoader loader;

    // Use this for initialization
    void Start()
    {
        loader = SplashContentLoader.GetLoader();
        StartCoroutine(LoadMain());
    }

    public IEnumerator LoadMain()
    {
        yield return new WaitForSeconds(waitTime);

        while (!loader.complete)
        {
            yield return new WaitForEndOfFrame();
        }

        LevelLoading.LevelLoadHandler.LoadLevel(LevelLoading.LevelIDs.MAIN);
    }
}
