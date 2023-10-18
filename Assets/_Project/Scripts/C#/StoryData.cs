using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DataService;

namespace ForgottenTrails.InkFacilitation
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