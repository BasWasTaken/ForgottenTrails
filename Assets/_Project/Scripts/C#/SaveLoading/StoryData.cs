using Bas.ForgottenTrails.SaveLoading;
using System;

namespace Bas.ForgottenTrails.InkConnections
{
    [Serializable]
    public class StoryData : DataClass
    {
        // Inspector Properties

        #region Public Constructors

        public StoryData() : base()
        {
        }

        #endregion Public Constructors

        #region Properties

        public string StoryStateJson { get; set; } = ""; // string indicating most recently saved state of the ink object.
        public string CurrentText { get; set; } = "";
        public string HistoryText { get; set; } = "";

        #endregion Properties

        // Constructor
    }
}