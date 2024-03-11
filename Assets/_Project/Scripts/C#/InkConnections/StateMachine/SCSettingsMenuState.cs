using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCSettingsMenuState : SCInGameMenuState
        {
            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.InGameMenu.pages.settingsPage.gameObject.SetActive(true);
                Controller.InterfaceBroker.InGameMenu.labels.settingsPageLabel.color = Color.black;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.InterfaceBroker.InGameMenu.pages.settingsPage.gameObject.SetActive(false);
                Controller.InterfaceBroker.InGameMenu.labels.settingsPageLabel.color = Color.white;
            }

            #endregion Public Methods

        }

        #endregion Classes
    }
}