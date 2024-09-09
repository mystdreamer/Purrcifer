using Purrcifer.Data.Defaults;
using UnityEngine;

public class BossTestScript : Boss
{
    internal override void ApplyWorldState(WorldStateEnum state)
    {

    }

    void Start()
    {
        UIManager.SetBossHealth(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            ((IEntityInterface)this).Health -= 10;
    }
}
