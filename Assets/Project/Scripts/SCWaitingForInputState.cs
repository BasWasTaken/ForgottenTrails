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
    partial class StoryController : MonoSingleton<StoryController>
    {
        partial class InterfaceBroking
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
                }
                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    // Debug.Log("Turning input on.");
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (InputReceived)
                    {
                        BaseState<StoryController> state;
                        if (Machine.CurrentState == Controller.waitingForChoiceState)
                        {
                            state = Controller.waitingForChoiceState;
                        }
                        else if (Machine.CurrentState == Controller.waitingForContinueState)
                        {
                            state = Controller.waitingForContinueState;
                        }
                        else
                        {
                            state = this;
                        }
                        Machine.DropState(state);
                    }
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
