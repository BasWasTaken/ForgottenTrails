using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCJournalMenuState : SCInGameMenuState
        {

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.InGameMenu.pages.journalPage.gameObject.SetActive(true);
                Controller.InterfaceBroker.InGameMenu.labels.journalPageLabel.color = Color.black;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.InterfaceBroker.InGameMenu.pages.journalPage.gameObject.SetActive(false);
                Controller.InterfaceBroker.InGameMenu.labels.journalPageLabel.color = Color.white;
            }

            #endregion Public Methods

        }

        #endregion Classes
    }
}