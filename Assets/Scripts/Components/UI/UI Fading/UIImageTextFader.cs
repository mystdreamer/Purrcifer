﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Purrcifer.UI
{
    /// <summary>
    /// Class responsible for fading UI Image Components. 
    /// </summary>
    public class UIImageTextFader : UIFader
    {

        /// <summary>
        /// The array of images to fade. 
        /// </summary>
        public Image[] images;

        /// <summary>
        /// The array of images to fade. 
        /// </summary>
        public TextMeshProUGUI[] texts;

        /// <summary>
        /// Set an alpha state. 
        /// </summary>
        public override float SetAlpha
        {
            set
            {
                _alphaCurrent = value;
                SetValue(value);
            }
        }

        /// <summary>
        /// Set the value to the elements in the array. 
        /// </summary>
        /// <param name="value"> The value to set. </param>
        internal override void SetValue(float value)
        {
            Color color = Color.white;
            
            foreach (Image item in images)
            {
                color = item.color;
                color.a = _alphaCurrent;
                item.color = color;
            }

            foreach (TextMeshProUGUI item in texts)
            {
                color = item.color;
                color.a = _alphaCurrent;
                item.color = color;
            }
        }
    }
}