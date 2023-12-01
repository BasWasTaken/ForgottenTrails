using DataService;
using ForgottenTrails.InkFacilitation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMain : MonoBehaviour
{
    public void ToMainMenu()
    {
        DataManager.Instance.ActiveDataDictionary.Clear();
        SceneManager.LoadScene(AssetManager.Instance.menuScene);
    }
}
