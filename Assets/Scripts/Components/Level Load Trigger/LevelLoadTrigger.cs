using Purrcifer.LevelLoading;
using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    public LevelID levelIDToLoad;
    private bool loading = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !loading)
        {
            loading = true;
            LevelLoadHandler.LoadLevel(levelIDToLoad);
        }
    }
}
