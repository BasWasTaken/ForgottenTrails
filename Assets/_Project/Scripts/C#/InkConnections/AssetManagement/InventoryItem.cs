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
    // This SHOULD be called ItemDefinition but if I change the name now then all the prefabs fucking break because unity is excellent (:
    public class InventoryItem : InkableObject
    {
        [field: SerializeField, BoxGroup("Info")]
        public string CanonicalName { get; private set; }

        public Ink.Runtime.InkListItem InkListItem { get; set; }
        public string description = "";
        public Sprite image;
        [Tooltip("Base Cost of the item")] public int coinValue;
        [Header("Skillchecks")]
        public int modifier;
        [field: SerializeField]
        public List<Affordance> Affordaces { get; private set; }
    }


}
