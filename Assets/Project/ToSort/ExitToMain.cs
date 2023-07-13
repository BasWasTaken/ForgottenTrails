using DataService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMain : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneManager.LoadScene(AssetManager.Instance.menuScene);
    }
}
