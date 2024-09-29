using UnityEngine;

public class Consumable_Heart : PowerupConsumable
{
    public override void OnApplicationEvent(GameObject player)
    {
        GameManager.Instance.PlayerState.Health += 1;
        gameObject.SetActive(false);
    }
}
