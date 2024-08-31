using NUnit.Framework;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Purrcifer.UI
{
    /// <summary>
    /// Class responsible for fading UI Image Components. 
    /// </summary>
    public class UIImageFader : MonoBehaviour
    {
        /// <summary>
        /// The current alpha of the images. 
        /// </summary>
        private float _alphaCurrent;

        /// <summary>
        /// The array of images to fade. 
        /// </summary>
        public Image[] images;

        public float SetAlpha
        {
            set
            {
                _alphaCurrent = value;
                SetValue(value);
            }
        }

        public delegate void FadeEvent();
        public FadeEvent fadeOpComplete;

        /// <summary>
        /// Called to fade in the linked objects. 
        /// </summary>
        /// <param name="callback"> Will call this interface when fade completes. </param>
        public void FadeIn() => StartCoroutine(FadeInCoroutine());

        /// <summary>
        /// Called to fade out the linked objects. 
        /// </summary>
        /// <param name="callback"> Will call this interface when fade completes. </param>
        public void FadeOut() => StartCoroutine(FadeOutCoroutine());

        /// <summary>
        /// Increases an objects alpha using interpolation. 
        /// </summary>
        /// <param name="callback"> The IFadeCallback to notify on completion. </param>
        private IEnumerator FadeInCoroutine()
        {
            while (_alphaCurrent < 1)
            {
                _alphaCurrent += Mathf.Lerp(0, 1, Time.deltaTime);
                SetValue(_alphaCurrent);
                yield return new WaitForEndOfFrame();
            }
            fadeOpComplete?.Invoke();
        }

        /// <summary>
        /// Decreases an objects alpha using interpolation. 
        /// </summary>
        /// <param name="callback"> The IFadeCallback to notify on completion. </param>
        private IEnumerator FadeOutCoroutine()
        {
            while (_alphaCurrent > 0)
            {
                _alphaCurrent -= Mathf.Lerp(0, 1, Time.deltaTime);
                SetValue(_alphaCurrent);
                yield return new WaitForEndOfFrame();
            }

            fadeOpComplete?.Invoke();
        }

        private void SetValue(float value)
        {
            Color color = Color.white;
            foreach (Image item in images)
            {
                color = item.color;
                color.a = _alphaCurrent;
                item.color = color;
            }
        }
    }
}