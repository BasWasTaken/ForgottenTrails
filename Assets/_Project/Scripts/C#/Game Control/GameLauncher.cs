using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using VVGames.Common;
using VVGames.ForgottenTrails.InkConnections;
using VVGames.ForgottenTrails.SaveLoading;

namespace VVGames.ForgottenTrails
{
    public class GameLauncher : MonoSingleton<GameLauncher>
    {
        #region Fields

        [Required]
        public TMPro.TMP_InputField profileNamer;

        [Required]
        public TMPro.TMP_Dropdown profileSelector;

        [Required]
        public TMPro.TMP_Dropdown saveSelector;

        #endregion Fields

        #region Public Methods

        public void StartNewGame()
        {
            StartNewGame(profileNamer.text);
        }

        public void StartNewGame(string fileName)
        {
            if (DataManager.Instance.TryStartNewGame(fileName)) LaunchGame();
        }

        public void ContinueGame()
        {
            DataManager.Instance.LoadMostRecent();
            LaunchGame();
        }

        // dev note: if I ever want the dropdown elements elsewhere, I can just separate these into disctinct components
        public void ShowProfiles()
        {
            profileSelector.ClearOptions();
            profileSelector.AddOptions(DataManager.Instance.DataProfiles);
            ShowSaves();
        }

        public void ShowSaves()
        {
            saveSelector.ClearOptions();
            saveSelector.AddOptions(DataManager.Instance.GetFilePaths(profileSelector.captionText.text));
        }

        public void LoadGame()
        {
            LoadGame(saveSelector.captionText.text);
        }

        public void LoadGame(string file)
        {
            DataManager.Instance.LoadDataFromFile(file);
            LaunchGame();
        }

        public void LaunchGame()
        {
            SceneManager.LoadScene(AssetManager.Instance.newGameScene, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        #endregion Protected Methods

        #region Private Methods

        private void Start()
        {
            int i = 0;
            do
            {
                i++;
                profileNamer.text = "UnnamedGame" + i;
            } while (DataManager.Instance.DataProfileExists(profileNamer.text));
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(AssetManager.Instance.newGameScene));
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            SceneManager.UnloadSceneAsync(AssetManager.Instance.menuScene);
            SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        }

        #endregion Private Methods
    }
}