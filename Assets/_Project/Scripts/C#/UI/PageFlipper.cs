using UnityEngine;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class PageFlipper : MonoBehaviour
    {
        ///___VARIABLES___///

        ///___METHODS___///
        ///

        #region Fields

        public TMPro.TextMeshProUGUI textBox;

        #endregion Fields

        #region Public Methods

        public void Flip(int dir = 1)
        {
            textBox.pageToDisplay += dir;
        }

        #endregion Public Methods
    }
}