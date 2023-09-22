using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// <para>Summary not provided.</para>
/// </summary>

public interface IMouseOverOption
{
    bool IsMouseOver { get; set; }
        void OnMouseEnter();
        void OnMouseExit();
        void UpdateWhenMouseOver();
}