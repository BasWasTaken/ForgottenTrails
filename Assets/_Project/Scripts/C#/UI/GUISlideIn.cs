
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Bas.ForgottenTrails.UI
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
        public Vector2Int direction;

        Vector3 displacement => 2 * direction * new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        #endregion
        #region backend
        private bool displaced = false;
        #endregion
        ///___METHODS___///
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
                transform.localPosition += displacement;
            }
        }
        public void Replace()
        {
            if (displaced)
            {
                displaced = false;
                transform.localPosition -= displacement;
            }
        }
    }
}