using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;
using UnityEngine.UI;

/// <summary>
/// <para>Summary not provided.</para>
/// </summary>
[RequireComponent(typeof(Button))]
public class MapItem : MonoBehaviour
{
    public string location;

    public Ink.Runtime.InkListItem InkListItem { get; set; }
    ///___METHODS___///
    ///
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=>ActivateFromButton());
    }
    public void ActivateFromButton()
    {
        StoryController.Instance.InterfaceBroker.TryTravelTo(this);
    }

}