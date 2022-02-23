using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject bar;
    public GameObject player;
    private float currentHealth;
    private float newBarScale;

    // Update is called once per frame
    void Update()
    {
        if(player.activeSelf)
            currentHealth = player.GetComponent<PlayerController>().GetPlayerHealth();
        newBarScale = currentHealth / 28.57f;

        bar.transform.localScale = new Vector3(
            newBarScale,
            bar.transform.localScale.y,
            bar.transform.localScale.z);
    }
}
