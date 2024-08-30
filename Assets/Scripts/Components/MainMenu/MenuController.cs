using UnityEngine;

public class MenuController : MonoBehaviour
{

    public void Start()
    {
        //Load in BTrees early. 
        //Load in GameManager early. 
    }
    
    public void NewGame()
    {
        Debug.Log("New game started.");
        LevelLoading.LevelLoadHandler.LoadLevel(LevelLoading.LevelIDs.LEVEL_1);
    }

    public void LoadGame()
    {
        Debug.Log("Not implemented: Loading game.");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        Debug.Log("Not implemented: Settings.");
    }
}
