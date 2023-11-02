using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BasUtility;
using DataService;
using UnityEngine.SceneManagement;
using Bas.Utility;

public class GameLauncher : MonoSingleton<GameLauncher>
{
    [Required]
    public TMPro.TMP_InputField profileNamer;
    [Required]
    public TMPro.TMP_Dropdown profileSelector;
    [Required]
    public TMPro.TMP_Dropdown saveSelector;
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
    public void StartNewGame()
    {
        StartNewGame(profileNamer.text);
    }
    public void StartNewGame(string fileName)
    {
        if (DataManager.Instance.TryStartNewGame(fileName))         LaunchGame();
    }
    public void ContinueGame()
    {
        DataManager.Instance.LoadMostRecent();
        LaunchGame();
    }
    // how single purpose should scripts be?
    // for instance, should these two methods be in a profileselector and saveselector component? it does make linking to them in the scene a bit more intuitive,
    // but i can't help but think that'd be a bit overkill...
    // well yeah i guess i should eb cause if i want them on any other buttons i don't want another whole gamelauncher!
    // soo i guess TODO: make these into separate components (low priority) 
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
