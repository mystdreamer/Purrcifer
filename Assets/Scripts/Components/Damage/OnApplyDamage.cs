using UnityEngine;

public class OnApplyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Collision Occured");
            GameManager.Instance.playerState.Health.Health -= 1;
        }
    }
}
