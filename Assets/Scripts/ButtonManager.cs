using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private int currentGreenAmmo;
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
        UpdateGreenAmmo();
        UpdateOrangeAmmo();
        UpdateCyanAmmo();
        UpdatePinkAmmo();
    }

    public void UpdateGreenAmmo()
    {
        currentGreenAmmo = 99;
        greenButtonText.text = "MAX";
    }
    public void UpdateOrangeAmmo()
    {
        currentOrangeAmmo = player.GetComponent<PlayerController>().GetCurrentOrangeAmmo();

        if (currentOrangeAmmo == maxOrangeAmmo)
            orangeButtonText.text = "MAX";

        else if (currentOrangeAmmo == 0)
            orangeButtonText.text = "EMPTY";

        else
            orangeButtonText.text = currentOrangeAmmo.ToString();
    }

    public void UpdateCyanAmmo()
    {
        currentCyanAmmo = player.GetComponent<PlayerController>().GetCurrentCyanAmmo();

        if (currentCyanAmmo == maxCyanAmmo)
            cyanButtonText.text = "MAX";

        else if (currentCyanAmmo == 0)
            cyanButtonText.text = "EMPTY";

        else
            cyanButtonText.text = currentCyanAmmo.ToString();
    }

    public void UpdatePinkAmmo() 
    {
        currentPinkAmmo = player.GetComponent<PlayerController>().GetCurrentPinkAmmo();

        if (currentPinkAmmo == maxPinkAmmo)
            pinkButtonText.text = "MAX";

        else if (currentPinkAmmo == 0)
            pinkButtonText.text = "EMPTY";

        else
            pinkButtonText.text = currentPinkAmmo.ToString();

    }
}
