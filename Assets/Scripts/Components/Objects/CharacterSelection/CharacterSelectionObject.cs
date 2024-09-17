using UnityEngine;

public class CharacterSelectionObject : MonoBehaviour
{
    public PlayerStartingStatsSO characterStats;

    void Start()
    {
        GameManager.DisableLevelTransition();
        PlayerMovementSys.UpdatePause = false;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            GameManager.SetPlayerData(characterStats);
            GameManager.LoadLevel(Purrcifer.LevelLoading.LevelID.LEVEL_1, false);
            gameObject.SetActive(false);
        }
    }
}
