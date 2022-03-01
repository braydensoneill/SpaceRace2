using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherPackController : MonoBehaviour
{
    // Pickup variables
    private float speed;
    private float maxRange;

    private void Start()
    {
        speed = 12.5f;  // Set the speed of the pickup
        maxRange = -20; // Set the max range of the pickup
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Always move forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        // Destroy the pickup if it exceeds its boundaries
        if (transform.position.x <= maxRange)
            Destroy(gameObject);
    }
}
