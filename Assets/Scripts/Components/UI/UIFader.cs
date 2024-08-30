using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public interface IFadeCallback
{
    void FadeOpComplete();
}

public class UIFader : MonoBehaviour
{
    public Image[] images;
    private float alphaCurrent;

    public void FadeIn(IFadeCallback callback)
    {
        StartCoroutine(FadeInCoroutine(callback));
    }

    public void FadeOut(IFadeCallback callback)
    {
        StartCoroutine(FadeOutCoroutine(callback));
    }

    private IEnumerator FadeInCoroutine(IFadeCallback callback)
    {
        Color color = Color.white;

        while (alphaCurrent < 1)
        {
            alphaCurrent += Mathf.Lerp(0, 1, Time.deltaTime);

            foreach (Image item in images)
            {
                color = item.color;
                color.a = alphaCurrent;
                item.color = color;
            }
            yield return new WaitForEndOfFrame();
        }
        callback.FadeOpComplete();
    }

    private IEnumerator FadeOutCoroutine(IFadeCallback callback)
    {
        Color color = Color.white;

        while (alphaCurrent > 0)
        {
            alphaCurrent -= Mathf.Lerp(0, 1, Time.deltaTime);

            foreach (Image item in images)
            {
                color = item.color;
                color.a = alphaCurrent;
                item.color = color;
            }
            yield return new WaitForEndOfFrame();
        }

        callback.FadeOpComplete();
    }
}