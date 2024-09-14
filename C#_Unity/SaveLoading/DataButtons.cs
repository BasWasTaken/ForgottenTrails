using UnityEngine;

namespace VVGames.ForgottenTrails.SaveLoading
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class DataButtons : MonoBehaviour
    {
        #region Public Methods

        public void QuickSave()
        {
            // ik wil via de state machine of story controller dit callen:
            DataManager.Instance.QuickSave();
        }

        public void QuickLoad()
        {
            // ik wil via de state machine of story controller dit callen:
            DataManager.Instance.QuickLoad();
            //StoryController.Instance.StateMachine.dropallstatesomehow;
            //StoryController.Instance.StateMachine.loadscene
            // then reset story scene
            // go through exiting then starting up the superstate?
        }

        public void DeleteAllData()
        {
            DataManager.Instance.WipeDataFromAllSlots();
        }

        #endregion Public Methods
    }
}