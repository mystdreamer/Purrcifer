using UnityEngine;

public class Consumable_Heart : AbsConsumable
{
    internal override void ConsumableEffect()
    {
        GameManager.Instance.playerState.AddHealth = 1;
        gameObject.SetActive(false);
    }
}
