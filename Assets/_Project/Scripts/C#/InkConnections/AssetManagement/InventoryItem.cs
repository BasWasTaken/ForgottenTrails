using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections.Items
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    [Serializable]
    [CreateAssetMenu]
    public class InventoryItem : InkableObject
    {
        #region Fields

        public string description = "";

        public Sprite image;

        [Tooltip("Base Cost of the item")] public int coinValue;

        [Header("Skillchecks")]
        public int modifier;

        #endregion Fields

        #region Properties

        [field: SerializeField, BoxGroup("Info")]
        public string CanonicalName { get; private set; }

        public Ink.Runtime.InkListItem InkListItem { get; set; }

        [field: SerializeField]
        public List<Affordance> Affordaces { get; private set; }

        #endregion Properties
    }
}