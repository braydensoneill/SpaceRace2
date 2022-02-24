using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //Add more here if you want to add extra scenes
    enum MenuOptions
    {
        Menu = 0,
        Play = 1,
    }

    public void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene((int)MenuOptions.Menu);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene((int)MenuOptions.Play);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
