using UnityEngine;

public class OnApplyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerState.Health -= 1;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.playerState.Health -= 1;
        }
    }
}
