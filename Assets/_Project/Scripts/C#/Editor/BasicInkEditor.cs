using Ink.Runtime;
using Ink.UnityIntegration;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
{
    /// <summary>
    /// An editor custom made to view the basic ink script in unity's editors.
    /// Taken from the ink demo at 2023-03-08, 14:50.
    /// </summary>
    [CustomEditor(typeof(StoryController))]
    [InitializeOnLoad]
    public class BasicInkEditor : NaughtyInspector
    {
        #region Public Constructors

        static BasicInkEditor()
        {
            StoryController.OnCreateStory += OnCreateStory;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void OnInspectorGUI()
        {
            Repaint();
            base.OnInspectorGUI();
            var realTarget = target as StoryController;
            var story = realTarget.Story;

            InkPlayerWindow.DrawStoryPropertyField(story, new GUIContent("Story"));
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCreateStory(Story story)
        {
            /// If you'd like NOT to automatically show the window and attach (your teammates may appreciate it!) then replace "true" with "false" here.
            InkPlayerWindow window = InkPlayerWindow.GetWindow(false);
            if (window != null) InkPlayerWindow.Attach(story);
        }

        #endregion Private Methods
    }
}