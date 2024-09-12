using JetBrains.Annotations;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_BossHealthBar : MonoBehaviour
{
    public BossHealth healthData;
    public GameObject panel;
    public Image image;
    public float lastValue;
    public float currentValue;
    public bool flickerActive = false;

    /// <summary>
    /// Set the boss to the UI_BossHealthBar. 
    /// </summary>
    public Boss SetCurrentBoss
    {
        set
        {
            //Setup scaling.
            healthData = value.BHealth;
            lastValue = healthData.MaxCap;
            image.transform.localScale = Vector3.one;
            panel.SetActive(true);
        }
    }

    public void Start()
    {
        panel.SetActive(false);
    }

    public void Update()
    {
        if (healthData == null)
            return; 

        lastValue = currentValue;
        currentValue = healthData.Health;
        CalculateDamageFlash();
    }

    private void CalculateDamageFlash()
    {
        if (currentValue < lastValue && !flickerActive)
        {
            float scaleValue = math.remap(0, healthData.MaxCap, 0, 1, currentValue);
            if (scaleValue < 0)
                scaleValue = 0;
            image.transform.localScale = new Vector3(scaleValue, 1);
            StartCoroutine(FlickerBar());
        }
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
