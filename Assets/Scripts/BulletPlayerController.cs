using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayerController : MonoBehaviour
{
    // Bullet info variables
    ScoreManager score;
    private float speed;
    private float maxRange;

    bool isColliding;

    void Awake()
    {
        score = FindObjectOfType<ScoreManager>();   // Used to reference the score manager
    }

    void Start()
    {
        speed = 75;     // Set the speed of the bullet
        maxRange = 20;  // Set the max range the bullet can travel
    }

    private void Update()
    {
        isColliding = false;    // Collision bug Fix
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();             // Move the bullet
        CheckOutOfBounds(); // Check if the bullet is out of bounds
    }
    private void Move()
    {
        // Always move the bullet forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void CheckOutOfBounds()
    {
        // Destroy the bullet if it exceeds its boundaries
        if (transform.position.x >= maxRange)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        // Check if the bullet collided with another ammunition
        if (col.gameObject.tag == "Ammunition" || col.gameObject.tag == "Repulsor")
        {
            Destroy(col.gameObject);    // Destroy the collider
            Destroy(gameObject);        // Destroy the bullet
        }

        // Check if the bullet collided with the enemy
        if (col.gameObject.tag == "Enemy")
        {
            if (isColliding) return;
            isColliding = true;
            score.AddScore(1);          // Add to the player's score
            Destroy(col.gameObject);    // Destroy the collider
            Destroy(gameObject);        // Destroy the bullet
        }
    }
}
