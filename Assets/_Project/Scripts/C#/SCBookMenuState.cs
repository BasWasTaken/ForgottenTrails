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
        public class SCBookMenuState : SCSuperState
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
                Controller.InterfaceBroker.inventory.book.Displace();
            }
            public override void OnUpdate()
            {
                base.OnUpdate();
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    ExitMenu();
                }
            }
            public override void OnExit()
            {
                Controller.InterfaceBroker.inventory.book.Replace();
            }
            #endregion
            // Private Methods
            #region Private Methods
            private void ExitMenu() {
                var state = Controller.StateMachine.CurrentState;
                if (StackBasedStateMachine<StoryController>.DoesXDescentFromY(state, this)) // this check should be redundant.... but is the current state a menu state?
                {
                    Controller.StateMachine.DropState(state);
                    
                }else
                {
                    throw new Exception("Unexpected State");
                }


                
            }

            #endregion
        }
    }
}