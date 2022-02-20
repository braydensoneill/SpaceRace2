using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private float speed;
    private float startPosition;
    private float endPosition;

    private void Start()
    {
        speed = 30;
        startPosition = 993;
        endPosition = -804;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 resetPosition = new Vector3(
            startPosition,
            transform.position.y,
            transform.position.z);

        transform.Translate(Vector3.left * Time.deltaTime * speed);  //Shoot laser forward from player

        if (transform.position.x <= endPosition)
            transform.position = resetPosition;    //Destroy the laser after it exceeds a certain range
    }
}
