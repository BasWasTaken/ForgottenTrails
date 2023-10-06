using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BasUtility;
using DataService;
using ForgottenTrails.InkFacilitation;

/// <summary>
/// <para>Summary not provided.</para>
/// </summary>
public class SavaDataButton : MonoBehaviour
{
    public void OnClick()
    {
        StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.savingState);  
    }
}