using UnityEngine;

public class PowerUp_Health : PowerupUtility
{
    public override void ApplyToPlayer(GameObject player)
    {
        gameObject.SetActive(false);
    }
}
