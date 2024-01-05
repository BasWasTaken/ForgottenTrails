using Bas.Common;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public partial class InterfaceBroking
        {
            #region Classes

            public class SCWaitingForContinueState : SCWaitingForInputState
            {
                // Inspector Properties

                // Public Properties

                // Private Properties

                // Public Methods

                #region Public Methods

                public override void OnEnter()
                {
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(true); // set bouncing triangle at most recent line
                }

                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (Controller.Story.canContinue)
                        {
                            DropCondition = true;
                            Machine.TransitionToState(Controller.savingState);
                        }
                    }
                }

                public override void OnExit()
                {
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(false); // remove marker
                }

                #endregion Public Methods

                // Private Methods
            }

            #endregion Classes
        }

        #endregion Classes
    }
}