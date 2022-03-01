using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorPlayerController : MonoBehaviour
{
    // Repulsor variables
    ScoreManager score;
    private float force;
    private float speed;
    private float maxRange;

    private void Awake()
    {
        // Allows reference to ScoreManager scipt
        score = FindObjectOfType<ScoreManager>();
    }

    void Start()
    {
        speed = 30;     // Set the speed of the repulsor
        maxRange = 20;  // Set the max range of the repulsor
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Travel();           // Move the repulsor
        CheckOutOfBounds(); // Check if it out of bounds
    }

    private void Travel()
    {
        // Always move forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void CheckOutOfBounds()
    {
        // Destroy the repulsor when it exceeds the boundary
        if (transform.position.x >= maxRange)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        // Check if the projectile collides with ammunition
        if (col.gameObject.tag == "Ammunition")
        {
            Destroy(col.gameObject);    // Destroy the collider
        }

        // Check if the projectile collides with an asteroid
        if (col.gameObject.tag == "Asteroid")
        {
            Destroy(col.gameObject);    // Destroy the collider
        }

        // Check if the projectile collides with an enemy
        if (col.gameObject.tag == "Enemy")
        {
            score.AddScore(1);          // Add to players score
            Destroy(col.gameObject);    // Destroy the collider
        }
    }
}
