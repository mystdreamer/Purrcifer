using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthBar : MonoBehaviour
{
    public GameObject panel;
    public Image image;
    public float bossHealthValue;

    private float ScaleHealthBar
    {
        set
        {
            float v = value; 
            float currentValue = math.remap(0, bossHealthValue, 0, 1, v);
            image.rectTransform.localScale = new Vector3(currentValue, 1);
        }
    }

    public void Start()
    {
        panel.SetActive(false);
    }

    public void SetUp(BossHealth bossHealth)
    {
        //Setup scaling. 
        bossHealthValue = bossHealth.Health;
        image.transform.localScale = Vector3.one;
        panel.SetActive(true);
    }

    public void UpdateState(float currentHealth) => ScaleHealthBar = currentHealth;
}
