using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light lightToFlicker; // Reference to the light to flicker
    public GameObject objectToFlicker; // Reference to the GameObject to flicker
    public float stableLightDurationMin = 0.5f; // Minimum time for stable light
    public float stableLightDurationMax = 2.0f; // Maximum time for stable light

    private void Start()
    {
        if (lightToFlicker == null)
        {
            lightToFlicker = GetComponent<Light>();
        }
        if (objectToFlicker == null)
        {
            objectToFlicker = gameObject;
        }
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            if (Random.value > 0.7f) // 30% chance to flicker
            {
                float flickerEndTime = Time.time + 1.0f; // Flicker for 1 second
                while (Time.time < flickerEndTime)
                {
                    bool isActive = !lightToFlicker.enabled;
                    lightToFlicker.enabled = isActive; // Toggle light on/off
                    objectToFlicker.SetActive(isActive); // Toggle GameObject on/off
                    yield return new WaitForSeconds(Random.Range(0.05f, 0.2f)); // Random interval for flickering
                }
                lightToFlicker.enabled = true; // Ensure light is on after flickering
                objectToFlicker.SetActive(true); // Ensure GameObject is active after flickering
            }
            else
            {
                lightToFlicker.enabled = true; // Keep light on
                objectToFlicker.SetActive(true); // Keep GameObject active
                float stableDuration = Random.Range(stableLightDurationMin, stableLightDurationMax);
                yield return new WaitForSeconds(stableDuration);
            }
        }
    }
}