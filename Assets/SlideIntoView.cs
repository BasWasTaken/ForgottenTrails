
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
    public class SlideIntoView : MonoBehaviour
    {
        ///___VARIABLES___///
        #region inspector
        [SerializeField]
        private float menuOffset =100f;
        #endregion
        #region backend
        private bool isMenuOpen = false;
        #endregion
        ///___METHODS___///
        public void ToggleOpen()
        {
            isMenuOpen = !isMenuOpen;
            if (isMenuOpen) StartCoroutine(Slide());
        }
        IEnumerator Slide()
        {
            for (int i = -10; i < 0; i++)
            {
                menuOffset = 10 * i;
                yield return new WaitForSeconds(0.1f);
            }
            menuOffset = 0;
        }

        void OnGUI()
        {
            if (isMenuOpen)
            {
                // Draw your menu here, but offset it by menuOffset:
                Rect rect = new Rect(0, menuOffset, 100, 100);
            }
        }
    }
}