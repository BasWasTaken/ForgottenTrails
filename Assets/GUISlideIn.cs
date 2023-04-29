
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NaughtyAttributes;
using Utility;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace MyGUI
{
    /// <summary>
    /// Allows gui elements to slide into view when clicked
    /// </summary>
    public class GUISlideIn : MonoBehaviour
    {
        ///___VARIABLES___///
        #region inspector
        [Required]
        [SerializeField] private RectTransform rectTransform;

        [MinValue(-1), MaxValue(1)]
        [OnValueChanged("CalculateDisplacement")]
        public Vector2Int direction;

        [ReadOnly]
        [SerializeField] Vector2 displacement;


        #endregion
        #region backend
        private bool displaced = false;
        #endregion
        ///___METHODS___///
        public void CalculateDisplacement()
        {
            displacement = direction * new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
        public void Toggle()
        {
            if (displaced)
            {
                Replace();
            }
            else
            {
                Displace();
            }
        }
        public void Displace()
        {
            if (!displaced)
            {
                displaced = true;
                transform.Translate(displacement);
            }
        }
        public void Replace()
        {
            if (displaced)
            {
                displaced = false;
                transform.Translate(-displacement);
            }
        }
    }
}