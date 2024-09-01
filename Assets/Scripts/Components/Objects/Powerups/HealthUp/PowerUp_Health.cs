public class PowerUp_Health : PowerUp_Item
{
    public const int HEALTH_INCREASE_VALUE = 1;

    internal override void ApplyPowerup()
    {
        GameManager.Instance.playerState.HealthMaxCap += HEALTH_INCREASE_VALUE;
        GameManager.Instance.playerState.AddHealth = HEALTH_INCREASE_VALUE;
    }
}
