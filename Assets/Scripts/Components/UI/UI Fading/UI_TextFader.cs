using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Purrcifer.UI
{

    /// <summary>
    /// Class responsible for fading Text UI Components. 
    /// </summary>
    public class UI_TextFader : UIFader
    {

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
            foreach (TextMeshProUGUI item in texts)
            {
                color = item.color;
                color.a = _alphaCurrent;
                item.color = color;
            }
        }
    }
}