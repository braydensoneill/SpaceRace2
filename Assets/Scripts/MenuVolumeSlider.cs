using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuVolumeSlider : MonoBehaviour
{
    // Volume variables
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Check if there is a sensitivity value saved from the last session and update accordingly
        CheckExistingVolume();
    }

    private void CheckExistingVolume()
    {
        // Check if there is no value saved from the last session
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.5f);   // The default sensitivity is 1
            FindVolume();                           // Update slider bar
        }

        else
        {
            FindVolume();   // Update slider bar
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;  // Set the game's volume to the slider value
        SaveVolume();                               // Save the volume playerpref to the slider value
    }

    private void FindVolume()
    {
        //Change the slider value to the player pref on startup
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void SaveVolume()
    {
        // Set the volume PlayerPref to the slider value
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}
