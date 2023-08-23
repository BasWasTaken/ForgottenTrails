using System;
using System.Collections;
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
        public class SCSavingState : SCSuperState
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
                if (!DropCondition)
                {
                    DataManager.Instance.OnDataSaved += Release;
                    Controller.SavingToDisk = true;

                    DataManager.Instance.WriteStashedDataToDisk();
                }
            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {
                DataManager.Instance.OnDataSaved -= Release;
            }
            #endregion
            // Private Methods
            #region Private Methods
            private void Release()
            {
                Controller.SavingToDisk = false;
                DropCondition = true;
            }
            #endregion
        }
    }

}
