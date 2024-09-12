using UnityEngine;

[System.Serializable]
public struct PowerupValue
{
    public float damageBase;
    public float damageMultiplier;
    public float damageCritcalHit;
    public float damageCritcalChance;
    public int healthCap;
    public bool refillHealth;
}

public abstract class Powerup : MonoBehaviour
{
    public PowerupValue powerupValue;
    public ItemDialogue itemDialogue;
    public bool eventAttached = false;

    public abstract string EventName { get; }
    public abstract int EventID { get; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Then update the players data. 
            UIManager.SetDialogue(itemDialogue);
            GameManager.Instance.ApplyPowerup(powerupValue);
            if (eventAttached)
                GameManager.Instance.SetPlayerDataEvent(EventName, EventID);
            gameObject.SetActive(false);
        }
    }
}
