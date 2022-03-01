using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyController : MonoBehaviour
{
    // Bullet info variables
    private float speed;
    private float maxRange;

    private void Start()
    {
        speed = 30;     // Set the speed of the bullet
        maxRange = -20; // Set the max range the bullet can travel
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Always move forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        // Destroy the bullet if it exceeds its boundaries
        if (transform.position.x <= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }
}
