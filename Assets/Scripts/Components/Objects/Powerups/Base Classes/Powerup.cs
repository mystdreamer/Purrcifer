using UnityEngine;

[System.Serializable]
public struct PowerupValue
{
    public float damageBase;
    public float damageMultiplier;
    public float damageCriticalHit;
    public float damageCriticalChance;
    public int healthCap;
    public bool refillHealth;
    public float playerSpeed; 
}

[System.Serializable]
public struct PlayerEventData
{
    public string name;
    public int id;
    public bool hasEvent;
}

public abstract class Powerup : MonoBehaviour
{
    public PowerupValue powerupValue;
    public ItemDialogue itemDialogue;
    public PlayerEventData eventData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Then update the players data. 
            UIManager.SetDialogue(itemDialogue);
            GameManager.Instance.ApplyPowerup(powerupValue);
            if (eventData.hasEvent)
                GameManager.Instance.SetPlayerDataEvent(eventData.name, eventData.id);
            OnApplication();
        }
    }

    public abstract void OnApplication();
}
