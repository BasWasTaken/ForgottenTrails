using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VVGames.ForgottenTrails.InkConnections.Items
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Forgotten Trails/Inventory Item")]
    public class InventoryItem : InkableObject
    {
        #region Fields

        [TextArea(minLines: 2, maxLines: 10)]
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

        #region Public Methods

        public bool ContainsAffordance(Affordance affordanceNeeded)
        {
            foreach (Affordance affordanceInItem in Affordaces)
            {
                if (affordanceInItem == affordanceNeeded)
                {
                    return true; // found it!
                }
            }
            return false; // did not find it!
        }

        public bool ContainsAffordance(string affordanceNeeded)
        {
            Affordance processed;
            if (Enum.TryParse(affordanceNeeded, out processed))
            {
                return ContainsAffordance(processed);
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}