using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VVGames.Common
{
    public class LoadEssentials : MonoBehaviour
    {
        #region Fields

        [Scene] public string alwaysOpenScene;

        #endregion Fields

        #region Private Methods

        private void Awake()
        {
            Scene scene = SceneManager.GetSceneByName(alwaysOpenScene);
            if (!scene.isLoaded)
            {
                SceneManager.LoadScene(alwaysOpenScene, LoadSceneMode.Additive);
            }
        }

        #endregion Private Methods
    }
}