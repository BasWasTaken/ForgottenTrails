using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.ForgottenTrails.Presentation
{
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    [Serializable]
    public class BackGround : MonoBehaviour
    {
        #region Fields

        ///___VARIABLES___///
        /// Serialized/Public, then private
        [SerializeField]
        private Sprite square;

        #endregion Fields

        #region Properties

        private Image[] Children { get; set; }
        private Image Front { get; set; }
        private Image Back { get; set; }

        #endregion Properties

        #region Public Methods

        public void SnapTo(Sprite sprite, Color color)
        {
            Front.sprite = sprite;
            Front.color = color;
            ToBack();
        }

        public void SnapTo(Sprite sprite)
        {
            SnapTo(sprite, Color.white);
        }

        public void SnapTo(Color color)
        {
            SnapTo(square, color);
        }

        public IEnumerator FadeTo(Sprite sprite, Color color, float duration)
        {
            //Debug.Log(String.Format("Started fading to a {0}-shaded image of {1}.", color, sprite));
            ToBack();
            if (duration <= 0)
            {
                SnapTo(sprite, color);
            }
            else
            {
                Front.sprite = sprite;
                Front.color = new(color.r, color.g, color.b, 0);
                while (Front.color.a < 1)
                {
                    Front.color = new Color(Front.color.r, Front.color.g, Front.color.b, Front.color.a + (Time.fixedDeltaTime / duration));
                    yield return new WaitForFixedUpdate();
                }
                ToBack();
            }
            //Debug.Log(String.Format("Done fading! Should now see a {0}-shaded image of {1}.", color, sprite));
        }

        public IEnumerator FadeTo(Sprite sprite, float duration)
        {
            return FadeTo(sprite, Color.white, duration);
        }

        public IEnumerator FadeTo(Color color, float duration)
        {
            return FadeTo(square, color, duration);
        }

        public IEnumerator FadeTo(string color, float duration)
        {
            System.Drawing.Color color1 = System.Drawing.Color.FromName(color);
            Color color2 = new(color1.R, color1.G, color1.B, color1.A);
            return FadeTo(color2, duration);
        }

        #endregion Public Methods

        #region Private Methods

        ///___LIFECYLE___///
        private void Awake()
        {
            Children = GetComponentsInChildren<Image>();
            Front = Children[^1]; // get lowest child
            Back = Children[0]; // get highest child
        }

        ///___FUNCTIONS___///
        private void ToBack() /// reset
        {
            Back.sprite = Front.sprite;
            Back.color = Front.color;

            Front.color = Color.clear;
            Front.sprite = null;
        }

        #endregion Private Methods
    }
}