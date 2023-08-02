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
            public class SCWaitingForContinueState : SCWaitingForInputState
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
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(true); // else set bouncing triangle at most recent line
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (Controller.Story.canContinue)
                        {
                            RegisterInput();
                            Machine.TransitionToState(Controller.savingState);
                        }
                    }
                }
                public override void OnExit()
                {
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(false); // remove marker 
                }
                #endregion
                // Private Methods
                #region Private Methods

                #endregion
            }
        }

    }
}