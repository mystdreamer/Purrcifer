using System.Collections;
using TMPro;
using UnityEngine;

namespace Purrcifer.UI
{
    [System.Serializable]
    public enum FadeState : int
    {
        OUT = 0, 
        IN = 1
    }

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

        public FadeState state = FadeState.OUT;

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
            float epsilon = 0.05F;

            do
            {
                _alphaCurrent += Mathf.Lerp(0, 1, Time.deltaTime);
                if (_alphaCurrent + epsilon >= 1)
                    _alphaCurrent = 1;
                SetValue(_alphaCurrent);
                Debug.Log("Fade In: " + _alphaCurrent);
                yield return new WaitForEndOfFrame();
            }
            while (_alphaCurrent < 1);

            state = FadeState.IN;

            Debug.Log("Fade In Completed and exited");
            fadeOpComplete?.Invoke();
        }

        /// <summary>
        /// Decreases an objects alpha using interpolation. 
        /// </summary>
        /// <param name="callback"> The IFadeCallback to notify on completion. </param>
        private IEnumerator FadeOutCoroutine()
        {
            float epsilon = 0.05F;
            while (_alphaCurrent > 0)
            {
                _alphaCurrent -= Mathf.Lerp(0, 1, Time.deltaTime);
                
                if(_alphaCurrent - epsilon <= 0)
                    _alphaCurrent = 0;

                SetValue(_alphaCurrent);
                Debug.Log("Fade Out: " + _alphaCurrent);
                yield return new WaitForEndOfFrame();

            }
            state = FadeState.OUT;
            fadeOpComplete?.Invoke();
        }

        /// <summary>
        /// Set the value to the elements in the array. 
        /// </summary>
        /// <param name="value"> The value to set. </param>
        internal abstract void SetValue(float value);
    }
}