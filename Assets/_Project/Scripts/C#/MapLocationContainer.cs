using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;
using UnityEngine.UI;

namespace Travel
{

    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MapLocationContainer : MonoBehaviour
    {
        public MapLocationDefinition definition;
        public string canonicalLocation => definition.CanonicalName;
        [HideInInspector]public Button Button;
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
