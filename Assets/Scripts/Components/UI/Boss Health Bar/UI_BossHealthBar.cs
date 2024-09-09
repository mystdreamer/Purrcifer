using JetBrains.Annotations;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthBar : MonoBehaviour
{
    public GameObject panel;
    public Image image;
    public float lastValue;
    public float bossHealthValue;
    public bool flickerActive = false;

    private float ScaleHealthBar
    {
        set
        {
            float v = value;
            lastValue = bossHealthValue;
            bossHealthValue = v;
            if (bossHealthValue < lastValue && !flickerActive)
            {
                StartCoroutine(FlickerBar());
            }
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
        lastValue = bossHealthValue;
        image.transform.localScale = Vector3.one;
        panel.SetActive(true);
    }

    private IEnumerator FlickerBar()
    {
        flickerActive = true;
        Color red = Color.red;
        Color norm = image.color;
        float time = 0;
        float flickerTime = 0.025f;

        while (time < flickerTime)
        {
            time += Time.deltaTime;
            image.color = Color.red;
            yield return new WaitForEndOfFrame();
            image.color = Color.white;
            yield return new WaitForEndOfFrame();
        }
        image.color = norm;
        flickerActive = false;

    }

    public void Deactivate()
    {
        panel.SetActive(false);
    }

    public void UpdateState(float currentHealth) => ScaleHealthBar = currentHealth;
}
