public class PowerUp_Health : Powerup
{
    public string eventName;
    public int eventID; 

    public override void OnApplication()
    {
        gameObject.SetActive(false);
    }
}
