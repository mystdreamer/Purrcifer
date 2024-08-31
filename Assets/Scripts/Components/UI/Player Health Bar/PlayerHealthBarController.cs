using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarController : MonoBehaviour
{
    public Image underlay;
    public Image[] images;

    public bool HealthBarEnabled { get; set; } = false;

    void Start()
    {
        if (!HealthBarEnabled)
        {
            underlay.enabled = false;
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null)
                {
                    images[i].enabled = false;
                }
            }
        }
    }


    public void UpdateHealthbar(int value)
    {
        if (underlay != null)
        {
            if (!underlay.enabled && HealthBarEnabled)
            {
                underlay.enabled = true;
            }

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null)
                {
                    if (i < value && HealthBarEnabled)
                    {
                        images[i].enabled = true;
                    }
                    else
                    {
                        images[i].enabled = false;
                    }
                }
            }
        }
    }
}
