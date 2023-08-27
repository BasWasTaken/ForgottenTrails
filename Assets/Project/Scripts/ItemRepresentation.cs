using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using items;
using ForgottenTrails.InkFacilitation;

/// <summary>
/// <para>Summary not provided.</para>
/// </summary>
public class ItemRepresentation : MonoBehaviour, IMouseOverOption
{
    public bool IsMouseOver { get; set; }
    public TMPro.TextMeshProUGUI prompt;
    [SerializeField]
    public InventoryItem definition;

    // Default implementation for the interface methods
    private void OnMouseEnter()
    {
        IsMouseOver = true;
        prompt.gameObject.SetActive(true);
        prompt.text = "";
    }

    private void OnMouseExit()
    {
        IsMouseOver = false;
        prompt.gameObject.SetActive(false);
    }
    private void OnMouseOver()
    {
        UpdateWhenMouseOver();
    }
    public void ActivateFromButton()
    {
        StoryController.Instance.InterfaceBroker.TryUseItem(definition);
    }
    private void UpdateWhenMouseOver()
    {
        prompt.transform.position = Input.mousePosition;
    }

    // Explicitly implementing the interface methods
    void IMouseOverOption.OnMouseEnter()
    {
        // You can call the default implementation if needed
        OnMouseEnter();
    }

    void IMouseOverOption.OnMouseExit()
    {
        // You can call the default implementation if needed
        OnMouseExit();
    }

    void IMouseOverOption.UpdateWhenMouseOver()
    {
        // You can call the default implementation if needed
        UpdateWhenMouseOver();
    }
}