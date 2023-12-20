using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

namespace Common.Utility
{
    public class LoadEssentials : MonoBehaviour
    {
        [Scene] public string alwaysOpenScene;
        private void Awake()
        {
            Scene scene = SceneManager.GetSceneByName(alwaysOpenScene);
            if (!scene.isLoaded)
            {
                SceneManager.LoadScene(alwaysOpenScene, LoadSceneMode.Additive);
            }
        }
    }
}


