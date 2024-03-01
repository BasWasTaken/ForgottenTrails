using NaughtyAttributes;
using System;
using UnityEngine;

namespace VVGames.ForgottenTrails.InkConnections.Party
{
    /// <summary>
    /// <para>Base type object for ingame items.</para>
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "PartyMember", menuName = "Forgotten Trails/Party Member")]
    public class PartyMemberSO : InkableObject
    {
        #region Fields

        public Sprite image;

        public string description;

        #endregion Fields

        #region Properties

        [field: SerializeField, BoxGroup("Info")]
        public string CanonicalName { get; private set; }

        public Ink.Runtime.InkListItem InkListItem { get; set; }

        #endregion Properties
    }
}