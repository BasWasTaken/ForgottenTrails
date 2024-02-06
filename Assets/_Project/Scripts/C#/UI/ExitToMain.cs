using VVGames.ForgottenTrails.InkConnections;
using VVGames.ForgottenTrails.SaveLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VVGames.ForgottenTrails.UI
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