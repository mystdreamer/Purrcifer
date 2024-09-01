using UnityEngine;

public class Consumable_Heart : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Collision Occured");
            GameManager.Instance.playerState.Health += 1;
            gameObject.SetActive(false);
        }
    }
}
