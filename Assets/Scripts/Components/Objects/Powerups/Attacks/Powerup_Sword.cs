using UnityEngine;

public class Powerup_Sword : PowerupWeapon
{
    public override void OnApplicationEvent(GameObject player)
    {
        if (GameManager.Instance.Player.GetComponent<SwordAttack>() == null)
            GameManager.Instance.Player.AddComponent<SwordAttack>();

        gameObject.SetActive(false);
    }
}
