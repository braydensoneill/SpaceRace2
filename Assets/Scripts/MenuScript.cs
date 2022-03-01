using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Enum contain possible scenes
    enum MenuOptions
    {
        Menu = 0,
        Play = 1,
    }

    public void Start()
    {
        // Set the volume to the saved volume from the previous session
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void MainMenu()
    {
        // Play the menu scene
        SceneManager.LoadScene((int)MenuOptions.Menu);
    }

    public void PlayGame()
    {
        // Play the PlayGame scene
        SceneManager.LoadScene((int)MenuOptions.Play);
    }

    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
    }
}