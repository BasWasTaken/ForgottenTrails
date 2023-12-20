using Bas.Common;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        /// <summary>
        /// 
        /// </summary>
        public class SCDataState : SCBookMenuState
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
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        }
    }
}