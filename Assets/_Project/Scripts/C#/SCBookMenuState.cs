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
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text.Contains("{UNITY:CloseMap}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.Continue();

                        break;
                    }
                    else if(choice.text.Contains("{UNITY:ClosePartyScreen}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.Continue();
                        break;
                    }
                }
                Controller.InterfaceBroker.inventory.book.Replace();
            }
            #endregion
            // Private Methods
            #region Private Methods
            public void ExitMenu()
            {

                //Debug.LogFormat("is {0}, a bookmenustate?", this); // simply "this" does not seem to work
                Controller.StateMachine.DropState(Machine.KnownStates[typeof(SCBookMenuState)]); // the state to drom from may be a child but not a parent
            }
            #endregion
        }
    }
}