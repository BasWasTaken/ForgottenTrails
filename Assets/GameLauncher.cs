using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Utility;
using DataService;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoSingleton<GameLauncher>
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
        SceneManager.UnloadSceneAsync(AssetManager.Instance.menuScene);
    }
}
