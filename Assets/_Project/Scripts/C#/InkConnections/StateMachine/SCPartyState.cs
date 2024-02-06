using Ink.Runtime;
using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        /// <summary>
        ///
        /// </summary>
        public class SCPartyState : SCBookMenuState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.book.pages.partyPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.partyMark.color = Color.clear;
                Controller.InterfaceBroker.partyScreen.FetchPartyMembers(Controller.Story.state.variablesState["Party"] as InkList);

                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text == "{UNITY:OpenPartyScreen}")
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);// hopelijk wordt ook dit niet dubbelop als je al van de visible optie komt.
                        Controller.Story.Continue();
                        Controller.InterfaceBroker.FindHiddenChoices();
                        break;
                    }
                }
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text.Contains("{UNITY:ClosePartyScreen}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.ContinueMaximally();
                        break;
                    }
                }
                Controller.InterfaceBroker.book.markers.partyMark.color = Color.white;
            }

            #endregion Public Methods

            // Private Methods
        }

        #endregion Classes
    }
}