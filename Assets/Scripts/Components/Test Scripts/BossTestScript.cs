using UnityEngine;

public class BossTestScript : BossHealth
{

    void Start()
    {
        UIManager.SetBossHealth(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            base.Health -= 10;
    }
}
