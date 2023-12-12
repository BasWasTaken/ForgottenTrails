using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
    
/// <summary>
/// <para>Summary not provided.</para>
/// </summary>
public class PageFlipper : MonoBehaviour
{
    ///___VARIABLES___///


    ///___METHODS___///
    ///

    public TMPro.TextMeshProUGUI textBox;

    public void Flip(int dir = 1)
    {
        textBox.pageToDisplay += dir;
    }
    
}