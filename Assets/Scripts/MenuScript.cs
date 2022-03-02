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
        // Check if there is a highscore playerpref saved from previous sessions
        if (PlayerPrefs.HasKey("volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
        
        // if there is no volume playerpref, set the volume to 1
        else
            AudioListener.volume = 1;

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