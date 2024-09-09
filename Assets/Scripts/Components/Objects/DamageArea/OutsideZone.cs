using Purrcifer.Data.Defaults;

public class OutsideZone : ZoneObject
{

    private void Update()
    {
        UpdateSize();
        if (insideArea && !ticking)
        {
            if (ticking == false && ObjectUpdatable)
            {
                ticking = true;
                GameManager.Instance.playerState.AddDamage = 1;
                StartCoroutine(CooldownTimer());
            }
        }
        else if (!insideArea && ticking)
        {
            StopCoroutine(CooldownTimer());
            ticking = false;
        }
    }

    internal override void SetWorldState(WorldStateEnum state) { }
}
