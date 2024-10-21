using UnityEngine;
using System.Collections;

public class FlashWhiteOnHit : MonoBehaviour
{
    public float flashDuration = 0.05f; // Duration of the flash effect
    private Renderer[] renderers;
    private Color[] originalEmissionColors;
    public int materialIndex = 1; // The material element index

    void Start()
    {
        // Get all Renderer components in this GameObject and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Initialize the original emission colors array
        originalEmissionColors = new Color[renderers.Length];

        // Store the original emission color for the specific material index
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].materials.Length > materialIndex)
            {
                if (renderers[i].materials[materialIndex].HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[i] = renderers[i].materials[materialIndex].GetColor("_EmissionColor");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            StartCoroutine(FlashEmission());
        }
    }

    private IEnumerator FlashEmission()
    {
        // Enable emission for the specific material index
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].materials.Length > materialIndex)
            {
                Material material = renderers[i].materials[materialIndex];
                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", Color.white);
                }
            }
        }

        // Wait for the duration of the flash
        yield return new WaitForSeconds(flashDuration);

        // Disable emission for the specific material index
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].materials.Length > materialIndex)
            {
                Material material = renderers[i].materials[materialIndex];
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", originalEmissionColors[i]);
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}
