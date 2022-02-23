using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    enum Direction
    {
        Forward = 0,
        Up = 1,
        Down = 2
    }

    public float speed;
    [SerializeField] private float maxRangeLeft;
    [SerializeField] private float maxRangeRight; //Some astroids may end up going right after collisions
    public int moveDirection;
    [SerializeField] private float bounceAngle;

    [SerializeField] private float currentXAngle;
    [SerializeField] private float currentYAngle;
    [SerializeField] private float currentZAngle;

    private Quaternion bounceUpRotation;
    private Quaternion bounceDownRotation;

    private float collisionRotationSmoothness;

    private void Start()
    {
        moveDirection = (int)Direction.Forward;
        maxRangeLeft = -20;
        maxRangeRight = 50;

        bounceAngle = 12.5f;
        collisionRotationSmoothness = 1;
    }

    private void Update()
    {
        currentXAngle = transform.rotation.eulerAngles.x;
        currentYAngle = transform.rotation.eulerAngles.y;
        currentZAngle = transform.rotation.eulerAngles.z;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Travel();
        CheckOutOfBounds();
    }

    private void Travel()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);  //Shoot laser forward from player
    }

    private void CheckRotation()
    {
        bounceUpRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + bounceAngle,
            transform.rotation.eulerAngles.z);

        bounceDownRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y - bounceAngle,
            transform.rotation.eulerAngles.z);

        switch (moveDirection)
        {
            case (int)Direction.Up:
                transform.rotation = Quaternion.Lerp(transform.rotation, bounceUpRotation, collisionRotationSmoothness);

                break;
            case (int)Direction.Down:
                transform.rotation = Quaternion.Lerp(transform.rotation, bounceDownRotation, collisionRotationSmoothness);
                break;
            default:
                transform.rotation = transform.rotation;
                break;
        }
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.x <= maxRangeLeft)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range

        if (transform.position.x >= maxRangeRight)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ammunition")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Asteroid")
        {
            if(transform.position.z > col.transform.position.z)
            {
                moveDirection = (int)Direction.Up;
                CheckRotation();
            }
            else
            {
                moveDirection = (int)Direction.Down;
                CheckRotation();
            }
        }
    }
}
