using UnityEngine;

public class BossTestScript : MonoBehaviour
{
    public BossHealth testBossHealth;

    void Start()
    {
        UIManager.SetBossHealth(testBossHealth);
    }

    void Update()
    {
        
    }
}
