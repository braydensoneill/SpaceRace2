using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        CheckExistingVolume();
        CheckExistingSensitivity();
    }

    private void CheckExistingVolume()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 2);
            LoadVolume();
        }

        else
        {
            LoadVolume();
        }
    }

    private void CheckExistingSensitivity()
    {
        if (!PlayerPrefs.HasKey("sensitivity"))
        {
            PlayerPrefs.SetFloat("sensitivity", 2);
            LoadSensitivity();
        }

        else
        {
            LoadSensitivity();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    public void ChangeSensitivity()
    {
        AudioListener.volume = sensitivitySlider.value;
        SaveSensitivity();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("perspective");
    }

    private void LoadSensitivity()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
    }
}
