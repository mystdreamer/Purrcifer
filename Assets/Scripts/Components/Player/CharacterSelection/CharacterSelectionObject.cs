using UnityEngine;

public class CharacterSelectionObject : MonoBehaviour
{
    public PlayerStartingStatsSO characterStats;
    public CharacterSelectionHandler characterSelectionHandler;
    bool loading = false; 


    public void OnCollisionEnter(Collision collision)
    {
        if (loading)
            return;

        if (collision.gameObject.tag == "Player")
        {
            characterSelectionHandler.OnSelection(characterStats);
        }
    }
}
