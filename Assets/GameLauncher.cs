using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Utility;
using DataService;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    [Required]
    public TMPro.TMP_Dropdown saveSlotSelector;
    public void StartNewGame()
    {
        StartNewGame(saveSlotSelector.value);
    }
    public void StartNewGame(int slot)
    {
        DataManager.Instance.NewGameOnSaveSlot(slot);
        LaunchGame();
    }
    public void ContinueGame()
    {
        ContinueGame(saveSlotSelector.value);
    }
    public void ContinueGame(int slot)
    {
        DataManager.Instance.ContinueFromSaveSlot(slot);
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
