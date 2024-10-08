﻿using System.Collections;
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
        private const float EPSILION = 0.05F;

        /// <summary>
        /// The current alpha of the images. 
        /// </summary>
        internal float _alphaCurrent;

        public FadeState state = FadeState.OUT;

        /// <summary>
        /// Set an alpha state. 
        /// </summary>
        public abstract float SetAlpha { set; }

        /// <summary>
        /// Called to fade in the linked objects. 
        /// </summary>
        /// <param name="callback"> Will call this interface when fade completes. </param>
        public void FadeIn()
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCoroutine());
        }

        /// <summary>
        /// Called to fade out the linked objects. 
        /// </summary>
        /// <param name="callback"> Will call this interface when fade completes. </param>
        public void FadeOut()
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutCoroutine());
        }

        /// <summary>
        /// Increases an objects alpha using interpolation. 
        /// </summary>
        /// <param name="callback"> The IFadeCallback to notify on completion. </param>
        private IEnumerator FadeInCoroutine()
        {
            do
            {
                _alphaCurrent += Mathf.Lerp(0, 1, Time.deltaTime);
                if (_alphaCurrent + EPSILION >= 1)
                    _alphaCurrent = 1;
                SetValue(_alphaCurrent);
                yield return new WaitForEndOfFrame();
            }
            while (_alphaCurrent < 1);

            state = FadeState.IN;
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

                if (_alphaCurrent - EPSILION <= 0)
                    _alphaCurrent = 0;

                SetValue(_alphaCurrent);
                yield return new WaitForEndOfFrame();

            }

            state = FadeState.OUT;
        }

        /// <summary>
        /// Set the value to the elements in the array. 
        /// </summary>
        /// <param name="value"> The value to set. </param>
        internal abstract void SetValue(float value);
    }
}