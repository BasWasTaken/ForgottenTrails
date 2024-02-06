using VVGames.Common;
using UnityEngine;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        /// <summary>
        ///
        /// </summary>
        public class SCDataState : SCBookMenuState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.book.pages.dataPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.dataMark.color = Color.clear;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.InterfaceBroker.book.markers.dataMark.color = Color.white;
            }

            #endregion Public Methods

            // Private Methods
        }

        #endregion Classes
    }
}