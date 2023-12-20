using Bas.ForgottenTrails.SaveLoading;
using System;

namespace Bas.ForgottenTrails.InkConnections
{
    [Serializable]
    public class StoryData : DataClass
    {
        // Inspector Properties
        #region Inspector Properties
        public string StoryStateJson { get; set; } = ""; // string indicating most recently saved state of the ink object.
        public string CurrentText { get; set; } = "";
        public string HistoryText { get; set; } = "";
        #endregion
        // Constructor
        #region Constructor
        public StoryData() : base()
        {

        }
        #endregion
    }
}