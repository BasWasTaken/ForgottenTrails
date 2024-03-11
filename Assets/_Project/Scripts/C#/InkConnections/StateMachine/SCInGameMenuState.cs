using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCInGameMenuState : SCSuperState
        {

            #region Public Methods

            public override void OnEnter()
            {
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    ExitMenu();
                }
                if (Controller.StateMachine.CurrentState == Machine.KnownStates[typeof(SCInGameMenuState)]) // "this" 
                {
                    ExitMenu(); // exit menu if there is no menu actually active except for this empty menu container
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
                    else if (choice.text.Contains("{UNITY:ClosePartyScreen}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.Continue();
                        break;
                    }
                }

                Controller.InterfaceBroker.InGameMenu.Supplemental.Clear();
            }


            public void ExitMenu()
            {
                //Debug.LogFormat("is {0}, a bookmenustate?", this); // simply "this" does not seem to work
                Controller.StateMachine.DropState(Machine.KnownStates[typeof(SCInGameMenuState)]); // the state to drop from may be a child but not a parent
            }

            #endregion Public Methods
        }

        #endregion Classes
    }
}