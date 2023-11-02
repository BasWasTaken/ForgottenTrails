using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        /// <summary>
        /// 
        /// </summary>
        public class SCMapState : SCBookMenuState
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties

            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {

                Controller.book.pages.mapPage.SetAsLastSibling();
                Controller.book.markers.mapMark.color = Color.clear;
            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {

                Controller.book.markers.mapMark.color = Color.white;
            }
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        }
    }
}