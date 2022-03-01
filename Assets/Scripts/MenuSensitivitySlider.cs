using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuSensitivitySlider : MonoBehaviour
{
    // Slider variables
    [SerializeField] Slider sensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        // Check if there is a sensitivity value saved from the last session and update accordingly
        CheckExistingSensitivity();
    }

    private void CheckExistingSensitivity()
    {
        // Check if there is no value saved from the last session
        if (!PlayerPrefs.HasKey("sensitivity"))
        {
            PlayerPrefs.SetFloat("sensitivity",  1);    // The default sensitivity is 1
            FindSensitivity();                          // Update slider bar
        }

        else
        {
            FindSensitivity();  // Update slider bar
        }
    }

    public void ChangeSensitivity()
    {
        SaveSensitivity(); // Save the sensitivity PlayerPref to the slider value
    }

    private void FindSensitivity()
    {
        // Change the slider value to the player pref on startup
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
    }

    private void SaveSensitivity()
    {
        // Set the sensitivity player pref to the silder value
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
    }
}
