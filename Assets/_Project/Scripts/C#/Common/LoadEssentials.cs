using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bas.Common
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


