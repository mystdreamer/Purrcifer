using System.Collections;
using TMPro;
using UnityEngine;

namespace Purrcifer.UI
{
    /// <summary>
    /// Class responsible for fading Text UI Components. 
    /// </summary>
    public abstract class UIFader : MonoBehaviour
    {
        /// <summary>
        /// The current alpha of the images. 
        /// </summary>
        internal float _alphaCurrent;

        /// <summary>
        /// Delegate used for fade event callbacks. 
        /// </summary>
        public delegate void FadeEvent();

        /// <summary>
        /// Event used to notify fade opeeration completion. 
        /// </summary>
        public FadeEvent fadeOpComplete;

        /// <summary>
        /// Set an alpha state. 
        /// </summary>
        public abstract float SetAlpha { set; }

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

        /// <summary>
        /// Set the value to the elements in the array. 
        /// </summary>
        /// <param name="value"> The value to set. </param>
        internal abstract void SetValue(float value);
    }
}