using UnityEngine;
using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCDataMenuState : SCInGameMenuState
        {

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.InGameMenu.pages.dataPage.gameObject.SetActive(true);
                Controller.InterfaceBroker.InGameMenu.labels.dataPageLabel.color = Color.black;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.InterfaceBroker.InGameMenu.pages.dataPage.gameObject.SetActive(false);
                Controller.InterfaceBroker.InGameMenu.labels.dataPageLabel.color = Color.white;
            }

            #endregion Public Methods

        }

        #endregion Classes
    }
}