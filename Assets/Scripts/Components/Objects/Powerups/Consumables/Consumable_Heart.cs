using UnityEngine;

public class Consumable_Heart : PowerupConsumable
{
    public override void ApplyToPlayer(GameObject player)
    {
        GameManager.Instance.PlayerState.AddHealth = 1;
        gameObject.SetActive(false);
    }
}