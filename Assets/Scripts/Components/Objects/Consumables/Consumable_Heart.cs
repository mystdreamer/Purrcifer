using UnityEngine;

public class Consumable_Heart : AbsConsumable
{
    internal override void ConsumableEffect()
    {
        GameManager.Instance.PlayerState.AddHealth = 1;
        gameObject.SetActive(false);
    }
}
