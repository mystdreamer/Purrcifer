using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashRedOnHit : MonoBehaviour
{
    public float flashDuration = 0.5f; // Duration of the flash effect
    public GameObject targetObject; // The target object to flash red
    public GameObject[] excludeObjects; // The child objects to exclude from flashing
    private Color originalColor;
    private Renderer[] renderers;

    void Start()
    {
        if (targetObject != null)
        {
            // Get all Renderer components in the target object and its children
            List<Renderer> rendererList = new List<Renderer>();
            foreach (Renderer rend in targetObject.GetComponentsInChildren<Renderer>())
            {
                // Exclude specified objects
                bool exclude = false;
                foreach (GameObject excludeObject in excludeObjects)
                {
                    if (rend.gameObject == excludeObject)
                    {
                        exclude = true;
                        break;
                    }
                }
                if (!exclude)
                {
                    rendererList.Add(rend);
                }
            }

            renderers = rendererList.ToArray();

            // Save the original color of the first renderer
            if (renderers.Length > 0)
            {
                originalColor = renderers[0].material.color;
            }
        }
        else
        {
            Debug.LogError("No target object assigned for FlashRedOnHit.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (targetObject != null &&
            (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
             collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile")))
        {
            StartCoroutine(FlashRed());
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (targetObject != null && other.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        // Change color to red
        foreach (var rend in renderers)
        {
            rend.material.color = Color.red;
        }

        // Wait for the duration of the flash
        yield return new WaitForSeconds(flashDuration);

        // Revert color back to original
        foreach (var rend in renderers)
        {
            rend.material.color = originalColor;
        }
    }
}
