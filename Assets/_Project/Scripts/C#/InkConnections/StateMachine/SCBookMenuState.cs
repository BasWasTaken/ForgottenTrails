using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCBookMenuState : SCSuperState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.inventory.book.Slide.Displace();
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
                    else if (choice.text.Contains("{UNITY:ClosePartyScreen}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.Continue();
                        break;
                    }
                }

                Controller.InterfaceBroker.inventory.book.RightPage.Clear();
                Controller.InterfaceBroker.inventory.book.Slide.Replace();
            }

            // Private Methods

            public void ExitMenu()
            {
                //Debug.LogFormat("is {0}, a bookmenustate?", this); // simply "this" does not seem to work
                Controller.StateMachine.DropState(Machine.KnownStates[typeof(SCBookMenuState)]); // the state to drop from may be a child but not a parent
            }

            #endregion Public Methods
        }

        #endregion Classes
    }
}