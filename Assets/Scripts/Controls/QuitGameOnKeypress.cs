using UnityEngine;
using System.Collections;

namespace Controls
{ 
  /// <summary>
  /// Simply provides functionality to close the deployed game executable. 
  /// </summary>
  /// Taken from: Ink demo, 2023-03-08, 14:39
	public class QuitGameOnKeypress : MonoBehaviour
	{

		public KeyCode key = KeyCode.Escape;

		void Update()
		{
			if (Input.GetKeyDown(key)) Application.Quit();
		}
	}
}

