using UnityEngine;

public class CharacterSelectionHandler : MonoBehaviour
{
    public bool canSelect = true;

    void Start()
    {
        GameManager.DisableLevelTransition();
        PlayerMovementSys.UpdatePause = false;
    }

    public void OnSelection(PlayerStartingStatsSO stats)
    {
        if (canSelect)
        {
            canSelect = false;
            GameManager.SetPlayerData(stats);
            GameManager.LoadLevel(Purrcifer.LevelLoading.LevelID.LEVEL_1, false);
            gameObject.SetActive(false);
        }
    }
}
