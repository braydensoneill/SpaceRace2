using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePlayerController : MonoBehaviour
{
    // Missile variables
    [SerializeField] private float speed;
    private float maxRange;

    private void Start()
    {
        speed = 30;     // Set the speed of the missile
        maxRange = 18;  // Set the max range of the missile
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Always move forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        // Destroy the missile if it exceeds the boundaries
        if (transform.position.x >= maxRange)
            Destroy(gameObject);
    }
}
