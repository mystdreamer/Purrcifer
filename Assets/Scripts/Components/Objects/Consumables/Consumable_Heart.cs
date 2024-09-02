using UnityEngine;

public class Consumable_Heart : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerState.AddHealth = 1;
            gameObject.SetActive(false);
        }
    }
}
