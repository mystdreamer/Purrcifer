using UnityEngine;

public class CharacterUnlockTrophy : MonoBehaviour
{
    private bool collected = false;

    private void OnCollisionEnter(Collision collision)
    {
        ResolveCollision(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        ResolveCollision(other.gameObject);
    }

    private void ResolveCollision(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            collected = true;
            bool glassUnlocked = DataManager.DataCarrier.GetEventState(2001);
            if (!glassUnlocked)
            {
                DataManager.DataCarrier.SetEventState(2001, true);
            }
            else
            {
                bool tankUnlocked = DataManager.DataCarrier.GetEventState(2002);
                if (!tankUnlocked)
                {
                    DataManager.DataCarrier.SetEventState(2002, true);
                }
            }

            GameManager.LoadLevel(Purrcifer.LevelLoading.LevelID.CHARACTER_SELECT, true);
        }
    }
}
