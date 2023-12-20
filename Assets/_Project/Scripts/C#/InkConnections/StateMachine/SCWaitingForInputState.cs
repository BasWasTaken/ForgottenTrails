using Bas.Common;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        public partial class InterfaceBroking
        {
            public class SCWaitingForInputState : SCSuperState
            {
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
                    if (!DropCondition)
                    {
                        // Debug.Log("Turning input on.");
                    }
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                }
                public override void OnExit()
                {
                    // Debug.Log("Turning input off.");
                }
                #endregion
                // Private Methods
                #region Private Methods

                #endregion
            }
        }
    }
}
