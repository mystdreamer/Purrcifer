using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_PlayerHealthBarController : MonoBehaviour
{
    public Image underlay;
    public Image[] heart_images;
    public Image[] heartUnderlay_images;

    public bool HealthBarEnabled { get; set; } = false;

    void Start()
    {
        if (!HealthBarEnabled)
        {
            SetImageState(heart_images, false);
            SetImageState(heartUnderlay_images, false);
            underlay.enabled = false;
        }
    }


    public void UpdateHealthBar(int current, int healthCap)
    {
        if (!HealthBarEnabled)
        {
            SetImageState(heart_images, false);
            SetImageState(heartUnderlay_images, false);
            underlay.enabled = false;
            return;
        }

        if (underlay != null)
        {
            if (!underlay.enabled && HealthBarEnabled)
                underlay.enabled = true;

            UpdateImageState(heartUnderlay_images, healthCap);
            UpdateImageState(heart_images, current);
        }
    }

    private void UpdateImageState(Image[] images, int value)
    {
        for (int i = 0; i < images.Length; i++)
            if (images[i] != null)
                images[i].enabled = (i < value && HealthBarEnabled);
    }

    private void SetImageState(Image[] images, bool state)
    {
        for (int i = 0; i < images.Length; i++)
            if (images[i] != null)
                images[i].enabled = state;
    }
}
