using NaughtyAttributes;
using System;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections.Travel
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    [Serializable]
    [CreateAssetMenu]
    public class MapLocationDefinition : InkableObject
    {
        #region Fields

        public string description = "";

        #endregion Fields

        #region Properties

        [field: SerializeField, BoxGroup("Info")]
        public string CanonicalName { get; private set; }

        public Ink.Runtime.InkListItem InkListItem { get; set; }

        #endregion Properties
    }
}