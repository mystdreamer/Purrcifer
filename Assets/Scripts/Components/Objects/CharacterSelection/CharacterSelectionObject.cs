using UnityEngine;

public class CharacterSelectionObject : MonoBehaviour
{
    public PlayerStartingStatsSO characterStats;
    bool loading = false; 

    void Start()
    {
        GameManager.DisableLevelTransition();
        PlayerMovementSys.UpdatePause = false;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (loading)
            return;

        if (collision.gameObject.tag == "Player")
        {
            loading = true;
            GameManager.SetPlayerData(characterStats);
            GameManager.LoadLevel(Purrcifer.LevelLoading.LevelID.LEVEL_1, false);
            gameObject.SetActive(false);
        }
    }
}
