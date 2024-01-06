using Bas.Common;
using Bas.ForgottenTrails.SaveLoading;

namespace Bas.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public class SCSavingState : SCSuperState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                if (!DropCondition)
                {
                    DataManager.Instance.OnDataSaved += Release;
                    Controller.SavingToDisk = true;

                    DataManager.Instance.SaveDataToFile(DataManager.SaveMethod.auto);
                }
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                Controller.SavingToDisk = false;
                DataManager.Instance.OnDataSaved -= Release;
            }

            #endregion Public Methods

            // Private Methods

            #region Private Methods

            private void Release()
            {
                Controller.SavingToDisk = false;
                DropCondition = true;
            }

            #endregion Private Methods
        }

        #endregion Classes
    }
}