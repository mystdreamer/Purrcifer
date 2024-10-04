using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage enemy deals

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.Player)
        {
            GameManager.Instance.PlayerState.Health -= 1;
            Debug.Log("Player takes damage: " + damageAmount);
        }
    }
}
