using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyController : MonoBehaviour
{
    private float speed;
    private float maxRange;

    private void Start()
    {
        speed = 30;
        maxRange = -20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);  //Shoot laser forward from player

        if (transform.position.x <= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }
}
