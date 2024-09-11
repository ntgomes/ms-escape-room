using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public void PlayGame()
    {
        // First, clear all of the necessary player prefs
        PlayerPrefs.DeleteKey("PlayerPos");
        PlayerPrefs.SetInt("SBComplete", 0);
        PlayerPrefs.SetInt("MBComplete", 0);
        PlayerPrefs.SetInt("RPComplete", 0);

        // Then load the scene
        SceneManager.LoadScene("Escape Room");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
