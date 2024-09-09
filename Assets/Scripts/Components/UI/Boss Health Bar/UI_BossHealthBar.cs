using JetBrains.Annotations;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_BossHealthBar : MonoBehaviour
{
    public BHealth healthData;
    public GameObject panel;
    public Image image;
    public float lastValue;
    public float currentValue;
    public bool flickerActive = false;

    public void Start()
    {
        panel.SetActive(false);
    }

    public void Update()
    {
        lastValue = currentValue;
        currentValue = healthData.Health;

        if (currentValue < lastValue && !flickerActive)
        {
            float scaleValue = math.remap(0, healthData.MaxCap, 0, 1, currentValue);
            if (scaleValue < 0)
                scaleValue = 0;
            image.transform.localScale = new Vector3(scaleValue, 1);
            StartCoroutine(FlickerBar());
        }
    }

    public void SetUp(Boss boss)
    {
        //Setup scaling.
        healthData = boss.BHealth;
        lastValue = healthData.MaxCap;
        image.transform.localScale = Vector3.one;
        panel.SetActive(true);
    }

    private IEnumerator FlickerBar()
    {
        flickerActive = true;
        Color red = Color.red;
        Color norm = image.color;
        float time = 0;
        float flickerTime = 0.005f;

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
        healthData = null;
        panel.SetActive(false);
    }
}
