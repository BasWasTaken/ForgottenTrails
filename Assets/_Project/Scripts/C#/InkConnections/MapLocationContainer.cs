using UnityEngine;
using UnityEngine.UI;

namespace VVGames.ForgottenTrails.InkConnections.Travel
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MapLocationContainer : MonoBehaviour
    {
        #region Fields

        public MapLocationDefinition definition;
        [HideInInspector] public Button Button;

        #endregion Fields

        #region Properties

        public string canonicalLocation => definition.CanonicalName;

        #endregion Properties

        #region Public Methods

        public void ActivateFromButton()
        {
            StoryController.Instance.InterfaceBroker.TryTravelTo(this);
        }

        #endregion Public Methods

        #region Private Methods

        ///___METHODS___///
        ///
        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(() => ActivateFromButton());
            //GetComponentInChildren<TMPro.TextMeshProUGUI>().text = canonicalLocation;
        }

        #endregion Private Methods
    }
}