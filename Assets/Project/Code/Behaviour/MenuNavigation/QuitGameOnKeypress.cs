using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Controls
{ 
  /// <summary>
  /// Simply provides functionality to close the deployed game executable. 
  /// </summary>
	public class QuitGameOnKeypress : MonoBehaviour
	{
		public KeyCode key = KeyCode.Escape;

		void Update()
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

