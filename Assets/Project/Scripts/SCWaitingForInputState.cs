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
        public partial class InterfaceBroking
        {
            public class SCWaitingForInputState : SCSuperState
            {
                // Public Properties
                #region Public Properties

                #endregion
                // Private Properties
                #region Private Properties

                protected bool InputReceived
                {
                    get
                    {
                        if (_inputReceived == true)
                        {
                            _inputReceived = false; // reset flag
                            return true;
                        }
                        else
                        {
                            return false;
                        };
                    }
                }
                private bool _inputReceived = false;
                protected void RegisterInput()
                {
                    _inputReceived = true;

                    DropCondition = true;
                }
                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    if (!DropCondition)
                    {
                        // Debug.Log("Turning input on.");
                    }
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                }
                public override void OnExit()
                {
                    // Debug.Log("Turning input off.");
                }
                #endregion
                // Private Methods
                #region Private Methods

                #endregion
            }
        }
    }
}
