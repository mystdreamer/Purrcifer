using UnityEngine;

public class Consumable : PowerupConsumable
{
    public override void OnApplicationEvent(GameObject player)
    {
        gameObject.SetActive(false);
    }
}
