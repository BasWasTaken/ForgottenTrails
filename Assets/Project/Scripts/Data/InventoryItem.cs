using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails;

namespace items
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    [Serializable]
    [CreateAssetMenu]
    public class InventoryItem : InkableObject
    {
        [Header("Info")]
        public string description;
        public Sprite image;
        [Tooltip("Base Cost of the item")] public int value;
        [Header("Skillchecks")]
        public int modifier;
        public List<Affordance> contexts;
    }

    public enum Affordance
    {
        Weapon,
        Tool,
        Cutting,
        Stabbing,
        Cooking
            //vul aan
    }



}
