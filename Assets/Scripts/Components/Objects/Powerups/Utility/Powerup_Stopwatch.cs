public class Powerup_Stopwatch : Powerup
{

    public override void OnApplication()
    {
        if (GameManager.Instance.Player.GetComponent<Stopwatch>() == null)
            GameManager.Instance.Player.AddComponent<Stopwatch>();

        gameObject.SetActive(false);
    }
}
