using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Cannon Positions
    public GameObject cannonLeft;
    public GameObject cannonFrontLeft;
    public GameObject cannonFront;
    public GameObject cannonFrontRight;
    public GameObject cannonRight;

    // Bullet prefab
    public GameObject bullet;

    // Enemy info variables 
    [SerializeField] private float speed;
    [SerializeField] private float maxRange;
    [SerializeField] private float fireRateTimer;
    [SerializeField] private float weaponFireRate;
    private float fireRate;

    // Variables for effects sound effects
    public float volume;
    private AudioSource enemyAudio;
    public AudioClip laserSound;
    public AudioClip explosionSound;

    // Variables used for particle effects
    public ParticleSystem explosionParticle;

    private void Awake()
    {
        // Variables for sound effects
        enemyAudio = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("volume");
    }
    private void Start()
    {
        speed = 12.5f;      // Set the speed of the enemy
        maxRange = -20;     // Set the max range of the enemy
        fireRate = 2.15f;   // Set the fire rate of the enemy
    }

    private void Update()
    {
        Charge();    // Used for checking if enough time has passed between shot

        // Check if the enemy is orange
        if (gameObject.name == "EnemyOrange(Clone)" || gameObject.name == "EnemyOrange")
        {
            // Shoot the orange weapon
            ShootOrange();
        }

        // Check if the enemy is pink
        else if (gameObject.name == "EnemyPink(Clone)" || gameObject.name == "EnemyPink")
        {
            // Shoot the pink weapon (uses shootNormal but will shoot the pink ammo attached to the enemy 
            ShootNormal();
        }

        // Or else shoot the normal weapon
        else
        {
            ShootNormal();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Always move forward
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        // Destroy the enemy if it has exceeded its boundaries
        if (transform.position.x <= maxRange)
            Destroy(gameObject);
    }

    private void ShootAudio()
    {
        // Play the laser sound
        enemyAudio.PlayOneShot(laserSound, volume * 1.3f);
    }

    private void ExplosionAudio()
    {
        // Play the explosion sound
        enemyAudio.PlayOneShot(explosionSound, volume * 20f);
    }

    private void ShootNormal()
    {
        // Check if the enemy is ready to shoot
        if (ReadyToShoot(fireRate))
        {
            ShootAudio();       // Play the shoot audio
            Instantiate(bullet, cannonFront.transform.position, transform.rotation);  //create new object with bullet prefab from left cannon
            fireRateTimer = 0;  //reset the timer
        }
    }

    private void ShootOrange()
    {
        float bulletSpread = 2.5f;    //Angle of orange bullet spread

        //Use the bullet spread variable to change the Y rotation of each bullet being shot
        Quaternion leftBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - 180 - bulletSpread,
            transform.rotation.z);

        Quaternion frontLeftBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - 180 - bulletSpread / 2,
            transform.rotation.z);

        Quaternion rightBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - 180 + bulletSpread,
            transform.rotation.z);

        Quaternion frontRightBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - 180 + bulletSpread / 2,
            transform.rotation.z);

        // Checl if the enemy is ready to shoot
        if (ReadyToShoot(fireRate))
        {
            // Play the shoot audio
            ShootAudio();

            //From each cannon used by this weapon, create a new object from the ammo's prefab, with the correct rotation
            Instantiate(bullet, cannonLeft.transform.position, leftBulletRotation);
            Instantiate(bullet, cannonFrontLeft.transform.position, frontLeftBulletRotation);
            Instantiate(bullet, cannonFront.transform.position, transform.rotation);
            Instantiate(bullet, cannonRight.transform.position, rightBulletRotation);
            Instantiate(bullet, cannonFrontRight.transform.position, frontRightBulletRotation);
            fireRateTimer = 0;    //reset the timer used for the fire rate
        }
    }

    private void Charge()
    {
        fireRateTimer += Time.deltaTime; // Start a timer
    }

    private bool ReadyToShoot(float rate)
    {
        // Check if the timer has exceed the fire rate
        if (fireRateTimer >= rate)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider col)
    {
        // Check if the enemy collided with an asteroid
        if(col.gameObject.tag == "Asteroid")
        {
            explosionParticle.Play();       // Play the explosion particle 
            ExplosionAudio();               // Play the explosion sound
            gameObject.SetActive(false);    // Disable the enemy
        }

        // Check if the enemy collided with ammunition or repulsors
        if(col.gameObject.tag == "Ammunition" || col.gameObject.tag == "Repulsor")
        {
            explosionParticle.Play();       // Play the explosion particle
            ExplosionAudio();               // Play the explosion sound
            gameObject.SetActive(false);    // Disable the enemy
        }
    }
}
