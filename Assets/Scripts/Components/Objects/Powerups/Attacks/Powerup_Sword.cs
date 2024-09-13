public class Powerup_Sword : Powerup
{

    public override void OnApplication()
    {
        if (GameManager.Instance.Player.GetComponent<SwordAttack>() == null)
            GameManager.Instance.Player.AddComponent<SwordAttack>();

        gameObject.SetActive(false);
    }
}
