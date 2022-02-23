using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    public GameObject greenButton;
    public GameObject orangeButton;
    public GameObject pinkButton;
    public GameObject cyanButton;

    public TextMeshProUGUI greenButtonText;
    public TextMeshProUGUI orangeButtonText;
    public TextMeshProUGUI pinkButtonText;
    public TextMeshProUGUI cyanButtonText;

    public GameObject player;

    private int currentOrangeAmmo;
    private int currentCyanAmmo;
    private int currentPinkAmmo;

    private int maxOrangeAmmo;
    private int maxCyanAmmo;
    private int maxPinkAmmo;

    private void Start()
    {
        maxOrangeAmmo = player.GetComponent<PlayerController>().GetMaxOrangeAmmo();
        maxCyanAmmo = player.GetComponent<PlayerController>().GetMaxCyanAmmo();
        maxPinkAmmo = player.GetComponent<PlayerController>().GetMaxPinkAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is alive
        if (player.activeSelf)
        {
            UpdateGreenAmmo();
            UpdateOrangeAmmo();
            UpdateCyanAmmo();
            UpdatePinkAmmo();
        }
    }

    public void UpdateGreenAmmo()
    {
        greenButtonText.text = "MAX";
    }
    public void UpdateOrangeAmmo()
    {
        currentOrangeAmmo = player.GetComponent<PlayerController>().GetCurrentOrangeAmmo();
        orangeButton.GetComponent<RawImage>().color = new Color32(255, 120, 0, 255);
        orangeButtonText.faceColor = new Color32(255, 120, 0, 255);

        if (currentOrangeAmmo == maxOrangeAmmo)
            orangeButtonText.text = "MAX";

        else if (currentOrangeAmmo == 0)
        {
            orangeButtonText.text = "0";
            orangeButton.GetComponent<RawImage>().color = new Color32(255, 120, 0, 50);
            orangeButtonText.faceColor = new Color32(255, 120, 0, 50);
        }

        else
            orangeButtonText.text = currentOrangeAmmo.ToString();
    }

    public void UpdateCyanAmmo()
    {
        currentCyanAmmo = player.GetComponent<PlayerController>().GetCurrentCyanAmmo();
        cyanButton.GetComponent<RawImage>().color = new Color32(0, 255, 255, 255);
        cyanButtonText.faceColor = new Color32(0, 255, 255, 255);

        if (currentCyanAmmo == maxCyanAmmo)
            cyanButtonText.text = "MAX";

        else if (currentCyanAmmo == 0)
        {
            cyanButtonText.text = "0";
            cyanButton.GetComponent<RawImage>().color = new Color32(0, 255, 255, 50);
            cyanButtonText.faceColor = new Color32(0, 255, 255, 50);
        }

        else
            cyanButtonText.text = currentCyanAmmo.ToString();
    }

    public void UpdatePinkAmmo() 
    {
        currentPinkAmmo = player.GetComponent<PlayerController>().GetCurrentPinkAmmo();
        pinkButton.GetComponent<RawImage>().color = new Color32(255, 0, 255, 255);
        pinkButtonText.faceColor = new Color32(255, 0, 255, 255);

        if (currentPinkAmmo == maxPinkAmmo)
            pinkButtonText.text = "MAX";

        else if (currentPinkAmmo == 0)
        {
            pinkButtonText.text = "0";
            pinkButton.GetComponent<RawImage>().color = new Color32(255, 0, 255, 50);
            pinkButtonText.faceColor = new Color32(255, 0, 255, 50);
        }

        else
            pinkButtonText.text = currentPinkAmmo.ToString();

    }
}
