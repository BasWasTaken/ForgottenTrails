using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BasUtility;
using DataService;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    [Required]
    public TMPro.TMP_InputField profileNamer;
    [Required]
    public TMPro.TMP_Dropdown profileSelector;
    public void StartNewGame()
    {
        StartNewGame(profileNamer.text);
    }
    public void StartNewGame(string fileName)
    {
        DataManager.Instance.TryStartNewGame(fileName);
        LaunchGame();
    }
    public void ContinueGame()
    {
        DataManager.Instance.LoadMostRecent();
        LaunchGame();
    }
    public void ShowSaves()
    {
        profileSelector.ClearOptions(); 
        profileSelector.AddOptions(DataManager.Instance.GetFilePaths());
    }
    public void LoadGame()
    {
        DataManager.Instance.LoadDataFromFile(profileSelector.itemText.text);
    }
    public void LoadGame(string file)
    {
        DataManager.Instance.LoadDataFromFile(file);
        LaunchGame();
    }
    private void LaunchGame()
    {
        SceneManager.LoadScene(AssetManager.Instance.newGameScene, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
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

}
