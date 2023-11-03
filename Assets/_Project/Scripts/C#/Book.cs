using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BasUtility;
using Bas.Utility;
using ForgottenTrails.InkFacilitation;
using static ForgottenTrails.InkFacilitation.StoryController;

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
    public void DropMenuState()
    {
        Type menuType = typeof(SCGameMenuState);
        var current = StoryController.Instance.StateMachine.CurrentState;
        Type currentType = current.GetType();
        if (menuType.IsAssignableFrom(currentType))
        {
            StoryController.Instance.StateMachine.DropState(current);
        }
    }

}