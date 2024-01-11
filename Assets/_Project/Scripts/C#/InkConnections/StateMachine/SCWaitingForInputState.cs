using VVGames.Common;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public partial class InterfaceBroking
        {
            #region Classes

            public class SCWaitingForInputState : SCSuperState
            {
                // Public Properties

                // Private Properties

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

                #endregion Public Methods

                // Private Methods
            }

            #endregion Classes
        }

        #endregion Classes
    }
}