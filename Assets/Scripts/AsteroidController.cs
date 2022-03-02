using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    ScoreManager score;

    enum Direction
    {
        Forward = 0,
        Up = 1,
        Down = 2
    }

    // Asteroid Info variables
    public float speed;
    public int moveDirection;

    // Boundary variables
    [SerializeField] private float maxRangeLeft;
    [SerializeField] private float maxRangeRight; //Some astroids may end up going right after collisions

    // Angles
    [SerializeField] private float currentXAngle;
    [SerializeField] private float currentYAngle;
    [SerializeField] private float currentZAngle;

    // Bounce variables
    [SerializeField] private float bounceAngle;
    private Quaternion bounceUpRotation;
    private Quaternion bounceDownRotation;
    private float collisionRotationSmoothness;

    //Variables for sound effects
    public float volume;
    private AudioSource asteroidAudio;
    public AudioClip explosionSound;

    // Variables used for particle effects
    public ParticleSystem explosionParticle;

    // Collision bug fix
    private bool isColliding;

    private void Awake()
    {
        // Variables for sound effects
        asteroidAudio = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("volume");
        score = FindObjectOfType<ScoreManager>();   // Used to reference the score manager
    }
    private void Start()
    {
        moveDirection = (int)Direction.Forward; // When spawned, move forward
        maxRangeLeft = -20;                     // Set boundary
        maxRangeRight = 60;                     // Set boundary

        bounceAngle = 12.5f;                    // Set the angle that the asteroid changes when they bounce
        collisionRotationSmoothness = 1;        // Set how smooth the rotate to their new angle when they bounce

        isColliding = false;
    }

    private void Update()
    {
        // Update angles
        currentXAngle = transform.rotation.eulerAngles.x; 
        currentYAngle = transform.rotation.eulerAngles.y;
        currentZAngle = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Travel();           // Move the asteroid
        CheckOutOfBounds(); // Check if the asteroid is out of bounds
    }

    private void Travel()
    {
        // Always move foward
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void CheckRotation()
    {
        // Change the rotation to this if it was the top asteroid
        bounceUpRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + bounceAngle,
            transform.rotation.eulerAngles.z);

        // Change the rotation to this if it was the bottom asteroid
        bounceDownRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y - bounceAngle,
            transform.rotation.eulerAngles.z);

        // Swict to the appropriate angle, the default will ensure nothing is changed
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

    private void ExplosionAudio()
    {
        // Play the explosion sound
        asteroidAudio.PlayOneShot(explosionSound, volume);
    }

    private void CheckOutOfBounds()
    {
        // Destroy the asteroid if it went too far to the left
        if (transform.position.x <= maxRangeLeft)
            Destroy(gameObject);

        // Destroy the asteroid if it went too far to the right
        if (transform.position.x >= maxRangeRight)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        // Check if the asteroid collided with ammunition
        if (col.gameObject.tag == "Ammunition")
        {
            Destroy(col.gameObject);    // Destroy the collider
        }

        // Check if the asteroid collided with repulsor
        if (col.gameObject.tag == "Repulsor")
        {
            explosionParticle.Play();   // Play an explosion particle
            ExplosionAudio();           // Play an explosion sound
            Destroy(col.gameObject);    // Destroy the collider
            Destroy(gameObject);        // Destroy the asteroid
        }

        // Check if the asteroid collided with an enemy
        if (col.gameObject.tag == "Enemy")
        {
            if (isColliding) return;
            isColliding = true;

            explosionParticle.Play();
            ExplosionAudio();           // Play an explosion sound
            Destroy(col.gameObject);    // Disable the collider
        }

        // Check if the asteroid collided with a player
        if (col.gameObject.tag == "Player")
        {
            //Disable the player in it's own script
            ExplosionAudio();   // Play an explosion sound
        }

        // Check if the asteroid collided with another asteroid
        if (col.gameObject.tag == "Asteroid")
        {
            // If this asteroid is above the other astroid, bounce up
            if(transform.position.z > col.transform.position.z)
            {
                moveDirection = (int)Direction.Up;
                CheckRotation();
            }

            // If this asteroid is below the other asteroid, bounce down
            else
            {
                moveDirection = (int)Direction.Down;
                CheckRotation();
            }
        }
    }
}
