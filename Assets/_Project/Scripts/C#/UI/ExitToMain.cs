using Bas.ForgottenTrails.InkConnections;
using Bas.ForgottenTrails.SaveLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bas.ForgottenTrails.UI
{
    public class ExitToMain : MonoBehaviour
    {
        #region Public Methods

        public void ToMainMenu()
        {
            DataManager.Instance.ActiveDataDictionary.Clear();
            SceneManager.LoadScene(AssetManager.Instance.menuScene);
        }

        #endregion Public Methods
    }
}