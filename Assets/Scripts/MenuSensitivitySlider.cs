using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuSensitivitySlider : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        CheckExistingSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckExistingSensitivity()
    {
        if (!PlayerPrefs.HasKey("sensitivity"))
        {
            PlayerPrefs.SetFloat("sensitivity",  1);
            FindSensitivity();
        }

        else
        {
            FindSensitivity();
        }
    }

    public void ChangeSensitivity()
    {
        //AudioListener.volume = sensitivitySlider.value;
        SaveSensitivity();
    }

    private void FindSensitivity()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
    }

    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
    }
}
