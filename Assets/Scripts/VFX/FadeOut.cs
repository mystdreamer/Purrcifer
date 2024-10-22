using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Time in seconds for the fade")]
    public float fadeDuration = 1.0f;

    [Tooltip("Should the object be destroyed after fading?")]
    public bool destroyAfterFade = false;

    [Tooltip("Should the fade start automatically?")]
    public bool autoStart = false;

    private Material[] materials;
    private float[] initialAlpha;
    private bool isFading = false;

    void Start()
    {
        // Get all materials from the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materials = renderer.materials;
            initialAlpha = new float[materials.Length];

            // Store initial alpha values
            for (int i = 0; i < materials.Length; i++)
            {
                // Enable transparency on all materials
                materials[i].SetFloat("_Surface", 1);
                materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                materials[i].SetInt("_ZWrite", 0);
                materials[i].renderQueue = 3000;
                materials[i].EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

                // Store initial alpha
                Color color = materials[i].color;
                initialAlpha[i] = color.a;
            }
        }

        if (autoStart)
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;

            // Update alpha for all materials
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a = Mathf.Lerp(initialAlpha[i], 0f, normalizedTime);
                materials[i].color = color;
            }

            yield return null;
        }

        // Ensure final alpha is exactly 0
        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = 0f;
            materials[i].color = color;
        }

        isFading = false;

        if (destroyAfterFade)
        {
            Destroy(gameObject);
        }
    }
}