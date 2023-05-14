using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Utility;
using DataService;
using UnityEngine.SceneManagement;

public class NewGameLauncher : MonoBehaviour
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
        SceneManager.LoadScene(AssetManager.Instance.newGameScene);
    }

}
