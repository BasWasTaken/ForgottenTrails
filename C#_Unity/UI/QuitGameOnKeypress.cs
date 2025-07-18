﻿using UnityEngine;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// Simply provides functionality to close the deployed game executable.
    /// </summary>
    public class QuitGameOnKeypress : MonoBehaviour
    {
        public KeyCode key = KeyCode.Escape;

        private void Update()
        {
            if (Input.GetKeyDown(key)) Quit();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}