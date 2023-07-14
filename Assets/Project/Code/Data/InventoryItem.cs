using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Core;

namespace items
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    public class InventoryItem : InkableObject
    {
        [Header("Info")]
        public string description;
        public Sprite image;
        [Tooltip("Base Cost of the item")] public int value;
        [Header("Skillchecks")]
        public int modifier;
        public List<Context> contexts;
    }

    public enum Context
    {
        Combat_Attacking,
        Combat_Defending,
        Speech,
        Crafting
    }
}
