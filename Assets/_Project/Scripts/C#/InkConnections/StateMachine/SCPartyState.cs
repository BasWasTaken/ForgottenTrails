using Bas.Common;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        /// <summary>
        ///
        /// </summary>
        public class SCPartyState : SCBookMenuState
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

                Controller.InterfaceBroker.book.pages.partyPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.partyMark.color = Color.clear;
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
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        }
    }
}