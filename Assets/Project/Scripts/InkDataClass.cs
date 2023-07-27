using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DataService;

namespace ForgottenTrails.InkFacilitation
{
    [Serializable]
    public class InkDataClass : DataClass
    {
        // Inspector Properties
        #region Inspector Properties
        public string StoryStateJson { get; set; } = ""; /// string indicating most recently saved state of the ink object.
        public SceneState SceneState { get; set; } = new();
        public string CurrentText { get; set; } = "";
        public string HistoryText { get; set; } = "";
        #endregion
        // Constructor
        #region Constructor
        public InkDataClass(string label) : base(label)
        {

        }
        #endregion
    }
    // Peripheal
    #region Peripheal
    [Serializable]
    public class SceneState
    {
        //Inspector Properties
        #region Inspector Properties
        public string Text { get; set; } = "null";
        public string History { get; set; } = "null";

        public string Background { get; set; } = "null";
        public string Sprites { get; set; } = "null";


        public string ActiveMusic { get; set; } = "null";
        public string ActiveAmbiance { get; set; } = "null";

        public float TextSpeedMod { get; set; } = 1;
        #endregion
    }
    #endregion
}