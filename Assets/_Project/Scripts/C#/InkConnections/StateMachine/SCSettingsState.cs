using Bas.Common;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCSettingsState : SCBookMenuState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.book.pages.settingPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.settingMark.color = Color.clear;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.InterfaceBroker.book.markers.settingMark.color = Color.white;
            }

            #endregion Public Methods

            // Private Methods
        }

        #endregion Classes
    }
}