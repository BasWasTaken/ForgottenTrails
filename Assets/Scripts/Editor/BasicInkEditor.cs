using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ink.UnityIntegration;
using Ink.Runtime;

namespace Core
{
    /// <summary>
    /// An editor custom made to view the basic ink script in unity's editors. 
    /// Taken from the ink demo at 2023-03-08, 14:50.
    /// </summary>
    [CustomEditor(typeof(BasicInkScript))]
    [InitializeOnLoad]
    public class BasicInkEditor : Editor
    {

        static BasicInkEditor()
        {
            BasicInkScript.OnCreateStory += OnCreateStory;
        }

        static void OnCreateStory(Story story)
        {
            /// If you'd like NOT to automatically show the window and attach (your teammates may appreciate it!) then replace "true" with "false" here. 
            InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
            if (window != null) InkPlayerWindow.Attach(story);
        }
        public override void OnInspectorGUI()
        {
            Repaint();
            base.OnInspectorGUI();
            var realTarget = target as BasicInkScript;
            var story = realTarget.story;
            InkPlayerWindow.DrawStoryPropertyField(story, new GUIContent("Story"));
        }
    }
}

