using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails.InkFacilitation;

/// <summary>
/// <para>Summary not provided.</para>
/// </summary>
public class Book : MonoBehaviour
{
    public void EnterSettingsState()
    {
        StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.settingsState);
    }
    public void EnterInventoryState()
    {
        StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.inventoryState);
    }

}