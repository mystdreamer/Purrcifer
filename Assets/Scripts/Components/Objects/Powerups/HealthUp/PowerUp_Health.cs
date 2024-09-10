public class PowerUp_Health : PowerUp_Item
{
    public const int HEALTH_INCREASE_VALUE = 1;

    internal override void ApplyPowerup()
    {
        GameManager.Instance.PlayerState.HealthMaxCap += HEALTH_INCREASE_VALUE;
        GameManager.Instance.PlayerState.AddHealth = HEALTH_INCREASE_VALUE;
    }
}
