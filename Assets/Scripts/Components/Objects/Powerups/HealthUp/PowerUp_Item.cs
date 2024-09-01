using UnityEngine;

/// <summary>
/// Abstract class responsible for generating powerups. 
/// </summary>
public abstract class PowerUp_Item : MonoBehaviour
{
    public Dialogue itemDialogue;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.SetDialogue(itemDialogue);
            ApplyPowerup();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Override and implement powerup code here. 
    /// </summary>
    internal abstract void ApplyPowerup();
}
