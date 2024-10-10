using UnityEngine;

public class Powerup_Stopwatch : PowerupUtility
{
    public override void OnApplicationEvent(GameObject player)
    {
        if (GameManager.Instance.Player.GetComponent<Stopwatch>() == null)
            GameManager.Instance.Player.AddComponent<Stopwatch>();

        gameObject.SetActive(false);
    }
}
