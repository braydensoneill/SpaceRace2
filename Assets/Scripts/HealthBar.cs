using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Health bar vairables
    public GameObject bar;
    public GameObject player;
    private float currentHealth;
    private float newBarScale;

    // Update is called once per frame
    void Update()
    {
        // Checkc if the player is active
        if (player.activeSelf)
        {   
            // Set the current health to the player's current health
            currentHealth = player.GetComponent<PlayerController>().GetPlayerHealth();
        }

        // Change the scale variables based on the player's current health
        newBarScale = currentHealth / 28.57f;

        //  Update the scales of the health bar accordingly
        bar.transform.localScale = new Vector3(
            newBarScale,
            bar.transform.localScale.y,
            bar.transform.localScale.z);
    }
}
