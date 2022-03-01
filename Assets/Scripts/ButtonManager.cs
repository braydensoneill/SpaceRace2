using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    // Buttons used
    public GameObject greenButton;
    public GameObject orangeButton;
    public GameObject pinkButton;
    public GameObject cyanButton;

    // Text on each button
    public TextMeshProUGUI greenButtonText;
    public TextMeshProUGUI orangeButtonText;
    public TextMeshProUGUI pinkButtonText;
    public TextMeshProUGUI cyanButtonText;

    // Used for referencing the player
    public GameObject player;

    // Current ammo variables
    private int currentOrangeAmmo;
    private int currentCyanAmmo;
    private int currentPinkAmmo;

    //Max ammo variables
    private int maxOrangeAmmo;
    private int maxCyanAmmo;
    private int maxPinkAmmo;

    private void Start()
    {
        // Set the max ammo variables to the player's max ammo variables 
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
            // Update each ammo type
            UpdateGreenAmmo();
            UpdateOrangeAmmo();
            UpdateCyanAmmo();
            UpdatePinkAmmo();
        }
    }

    public void UpdateGreenAmmo()
    {
        // The green ammo is always full, so display MAX at all times
        greenButtonText.text = "MAX";
    }
    public void UpdateOrangeAmmo()
    {
        // Find the player's current orange ammo 
        currentOrangeAmmo = player.GetComponent<PlayerController>().GetCurrentOrangeAmmo();

        // Set the colours of the button and text
        orangeButton.GetComponent<RawImage>().color = new Color32(255, 120, 0, 255);
        orangeButtonText.faceColor = new Color32(255, 120, 0, 255);

        // If the ammo is full, display MAX
        if (currentOrangeAmmo == maxOrangeAmmo)
            orangeButtonText.text = "MAX";

        // If the ammo is empty, display 0 and gery-out the button
        else if (currentOrangeAmmo == 0)
        {
            orangeButtonText.text = "0";
            orangeButton.GetComponent<RawImage>().color = new Color32(255, 120, 0, 50);
            orangeButtonText.faceColor = new Color32(255, 120, 0, 50);
        }

        // Or else display the current ammo
        else
            orangeButtonText.text = currentOrangeAmmo.ToString();
    }

    public void UpdateCyanAmmo()
    {
        // Find the player's current cyan ammo
        currentCyanAmmo = player.GetComponent<PlayerController>().GetCurrentCyanAmmo();

        // Set the colours of the button and text
        cyanButton.GetComponent<RawImage>().color = new Color32(0, 255, 255, 255);
        cyanButtonText.faceColor = new Color32(0, 255, 255, 255);

        // If ammo is full, display MAX
        if (currentCyanAmmo == maxCyanAmmo)
            cyanButtonText.text = "MAX";

        else if (currentCyanAmmo == 0)
        {
            cyanButtonText.text = "0";
            cyanButton.GetComponent<RawImage>().color = new Color32(0, 255, 255, 50);
            cyanButtonText.faceColor = new Color32(0, 255, 255, 50);
        }

        // Or else display the current ammo
        else
            cyanButtonText.text = currentCyanAmmo.ToString();
    }

    public void UpdatePinkAmmo() 
    {
        // Find the player's current pink ammo
        currentPinkAmmo = player.GetComponent<PlayerController>().GetCurrentPinkAmmo();

        // Set the colours of the button and text
        pinkButton.GetComponent<RawImage>().color = new Color32(255, 0, 255, 255);
        pinkButtonText.faceColor = new Color32(255, 0, 255, 255);

        // If ammo is full, display MAX
        if (currentPinkAmmo == maxPinkAmmo)
            pinkButtonText.text = "MAX";

        // If ammo is empty, display 0 and grey-out button and text
        else if (currentPinkAmmo == 0)
        {
            pinkButtonText.text = "0";
            pinkButton.GetComponent<RawImage>().color = new Color32(255, 0, 255, 50);
            pinkButtonText.faceColor = new Color32(255, 0, 255, 50);
        }

        // Or else display the current ammo

        else
            pinkButtonText.text = currentPinkAmmo.ToString();
    }
}
