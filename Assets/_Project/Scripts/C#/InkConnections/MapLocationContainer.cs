using UnityEngine;
using UnityEngine.UI;

namespace Bas.ForgottenTrails.InkConnections.Travel
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MapLocationContainer : MonoBehaviour
    {
        public MapLocationDefinition definition;
        public string canonicalLocation => definition.CanonicalName;
        [HideInInspector] public Button Button;
        ///___METHODS___///
        ///
        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(() => ActivateFromButton());
            //GetComponentInChildren<TMPro.TextMeshProUGUI>().text = canonicalLocation;
        }
        public void ActivateFromButton()
        {
            StoryController.Instance.InterfaceBroker.TryTravelTo(this);
        }
    }
}
