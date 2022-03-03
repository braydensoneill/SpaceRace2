using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour
{
    //Enum containing each weapon type
    enum Weapons
    {
        green = 1,
        orange = 2,
        pink = 3,
        cyan = 4,
    }

    //Player Info Variables
    private float maxPlayerHealth;
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerSpeed;

    //Variables used for passive healing
    private float healAmount;
    private float healFrequency;

    //Variables used to rotate the player spirte during movement
    Quaternion rotationTarget;
    public GameObject playerSprite;

    //Weapon Generic Variables
    public GameObject cannonLeft;
    public GameObject cannonFrontLeft;
    public GameObject cannonFront;
    public GameObject cannonFrontRight;
    public GameObject cannonRight;

    public GameObject greenBullet;
    public GameObject orangeBullet;
    public GameObject pinkBullet;
    public GameObject cyanBullet;

    [SerializeField] private int currentOrangeAmmo;
    [SerializeField] private int currentPinkAmmo;
    [SerializeField] private int currentCyanAmmo;

    [SerializeField] private int maxOrangeAmmo;
    [SerializeField] private int maxPinkAmmo;
    [SerializeField] private int maxCyanAmmo;

    [SerializeField] private int currentWeapon;
    [SerializeField] private float weaponFireRate;

    //Weapon Fire Rate Variables
    [SerializeField] private float fireRateGreen;
    [SerializeField] private float fireRateOrange;
    [SerializeField] private float fireRateCyan;
    [SerializeField] private float fireRatePink;

    //Player Controls Variables
    private float verticalInput;
    private int minZBoundary;
    private int maxZBoundary;

    // Timer variables
    private float healTimer;
    [SerializeField] private float fireRateTimer;

    // GUI references
    public GameObject topHUD;
    public GameObject centreHUD;
    public GameObject bottomHUD;

    // Variables for writing to file
    ScoreManager scoreScript;

    //Variables for sound effects
    public float volume;
    private AudioSource playerAudio;
    public AudioClip laserSound;
    public AudioClip explosionSound;

    // Variables used for particle effects
    public ParticleSystem explosionParticle;

    bool isColliding;

    private void Awake()
    {
        //Player variables
        maxPlayerHealth = 100;

        //Passive healing variables
        healAmount = 1f;
        healFrequency = 0.15f;

        //Max Ammo Variables
        maxOrangeAmmo = 5;
        maxPinkAmmo = 3;
        maxCyanAmmo = 150;

        //Fire Rate Variables
        fireRateGreen = 0.45f;
        fireRateOrange = 0.45f;
        fireRatePink = 0.45f;
        fireRateCyan = 0.01f;

        //Player Control Variables
        minZBoundary = -5;
        maxZBoundary = 7;

        //Timer variables
        fireRateTimer = 0f;
        healTimer = 0f;

        // Variables for writing to file
        scoreScript = FindObjectOfType<ScoreManager>();

        // Variables for sound effects
        playerAudio = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("volume");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Player Info Variables
        playerHealth = maxPlayerHealth;
        SetSpeed();

        //Player Weapon Variables
        currentWeapon = (int)Weapons.green;

        //Current Ammo variables
        currentOrangeAmmo = 0;
        currentPinkAmmo = 0;
        currentCyanAmmo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FindInputValues();                      // Find the values used for player input
        Debug.Log("Vertical Input: " + verticalInput);
        PassiveHeal(healAmount, healFrequency); // Every x seconds, heal for x 
        Shoot();                                // Allow the player to shoot
        Charge();                               // Check if enough time has passed since the last shot
        ChangeWeapon();                         // Allow the player to switch guns
        isColliding = false;                    // Collision bug fix
     }

    private void FixedUpdate()
    {
        Movement(); // The player can move up and down within the game's boundaries
        Rotation(); // The player's sprite swivels when the player moves
    }

    private void LateUpdate()
    {
        Die();  // Check of the player has dies at the end of each frame
    }

    private void SetSpeed()
    {
        // Check if there is a highscore playerpref saved from previous sessions
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            playerSpeed = 25 * PlayerPrefs.GetFloat("sensitivity"); //Default sensitivity is 30, but is changed based on playerPref value
        }

        // If the player has no highscore saved, the default value is 0
        else
            playerSpeed = 25;
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetCurrentOrangeAmmo()
    {
        return currentOrangeAmmo;
    }

    public int GetCurrentCyanAmmo()
    {
        return currentCyanAmmo;
    }

    public int GetCurrentPinkAmmo()
    {
        return currentPinkAmmo;
    }

    public int GetMaxOrangeAmmo()
    {
        return maxOrangeAmmo;
    }

    public int GetMaxCyanAmmo()
    {
        return maxCyanAmmo;
    }

    public int GetMaxPinkAmmo()
    {
        return maxPinkAmmo;
    }

    private void FindInputValues()
    {
        //If running on android, use the phone's gyroscope as the controller
        if(Application.platform == RuntimePlatform.Android)
            verticalInput = Input.acceleration.x;

        //If running on computer, use WASD/Arrow keys for movement
        else
            verticalInput = Input.GetAxis("Vertical");
    }

    private void Movement()
    {
        // Move the player up/down based on player input and speed
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed * verticalInput);

        // Prevent the player from moving past the top boundary
        if (transform.position.z < minZBoundary)
            transform.position = new Vector3(transform.position.x, transform.position.y, minZBoundary);

        // Prevent the player from moving past the bottom boundary
        if (transform.position.z > maxZBoundary)
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZBoundary);
     }

    public void Rotation()
    {
        // Use a variables that determine how intentively the player rotates
        float rotationStrength = 50.0f;

        // Create a new Quaternion that holds the players rotation based on certain parameters
        Quaternion playerSpriteRotation = Quaternion.Euler(
            playerSprite.transform.rotation.x + (verticalInput * rotationStrength),
            playerSprite.transform.rotation.y,
            playerSprite.transform.rotation.z); ;

        // Set the player's rotation to this new Quaternion value
        playerSprite.transform.rotation = playerSpriteRotation;
    }

    private bool CheckShootKeybind()
    {
        // Check if the player wanted to shoot their weapon
        if ((Input.GetMouseButton(0)) ||                                                // Did the player press the mouse button?
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||    // Did the player touch the screen?
            (Input.GetKey(KeyCode.Space)))                                              // Did the player press space bar?
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void Shoot()
    {
        //Check if the touch/mouse button is currently being held down
        if (CheckShootKeybind() == true)
        {
            //Check which gun the player is currently using and shoot the respective weapon
            switch (currentWeapon)
            {
                case (int)Weapons.green: ShootGreen(); break;
                case (int)Weapons.orange: ShootOrange(); break;
                case (int)Weapons.pink: ShootPink(); break;
                case (int)Weapons.cyan: ShootCyan(); break;
                default: ShootGreen(); break;
            }
        }
    }

    private void ShootAudio()
    {
        // Play a laser sound
        playerAudio.PlayOneShot(laserSound, volume);
    }

    private void ShootGreen()
    {
        // Change the player's fire rate to the fire rate of their respective weapon
        weaponFireRate = fireRateGreen;

        // Check if enough time has passed since the last shot
        if (ReadyToShoot(weaponFireRate))
        {
            // Play the Audio of the laser being shot
            ShootAudio();

            // Create a new object with Laser prefab from the left cannon
            Instantiate(greenBullet, cannonFront.transform.position, transform.rotation);

            // Reset the timer of the gun's fire rate
            fireRateTimer = 0;
        }
    }

    void ShootOrange()
    {
        float bulletSpread = 2.5f;          //Angle of shotgun bullet spread
        weaponFireRate = fireRateOrange;    //Change the player's current fire rate to the shotgun's fire rate

        //Use the bullet spread variable to change the Y rotation of each bullet being shot
        Quaternion leftBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - bulletSpread,
            transform.rotation.z);

        Quaternion frontLeftBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y - bulletSpread/2,
            transform.rotation.z);

        Quaternion rightBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y + bulletSpread,
            transform.rotation.z);

        Quaternion frontRightBulletRotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y + bulletSpread/2,
            transform.rotation.z);

        // Check if enough time has passed since the last shot and if there is enough ammo
        if (ReadyToShoot(weaponFireRate) && (currentOrangeAmmo > 0))
        {
            // Play the Audio of the laser being shot
            ShootAudio();

            // From each cannon used by this weapon, create a new object from the ammo's prefab, with the correct rotation
            Instantiate(orangeBullet, cannonLeft.transform.position, leftBulletRotation);
            Instantiate(orangeBullet, cannonFrontLeft.transform.position, frontLeftBulletRotation);
            Instantiate(orangeBullet, cannonFront.transform.position, transform.rotation);
            Instantiate(orangeBullet, cannonRight.transform.position, rightBulletRotation);
            Instantiate(orangeBullet, cannonFrontRight.transform.position, frontRightBulletRotation);

            currentOrangeAmmo--;    // Reduce the amount of ammo in the orange gun
            fireRateTimer = 0;      // Reset the timer used for the fire rate
        }

        // Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentOrangeAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    void ShootPink()
    {
        // Change the player's fire rate to the fire rate of their respective weapon
        weaponFireRate = fireRatePink;

        // Check if enough time has passed since the last shot and if the player has enough ammo
        if (ReadyToShoot(weaponFireRate) && (currentPinkAmmo > 0))
        {
            // Play the Audio of the laser being shot
            ShootAudio();

            // Create new object with Laser prefab from left cannon
            Instantiate(pinkBullet, cannonFront.transform.position, transform.rotation);

            currentPinkAmmo--;      // Reduce the amount of ammo in the pink gun
            fireRateTimer = 0;      // Reset the timer used for the fire rate
        }

        //Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentPinkAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    void ShootCyan()
    {
        // Change the player's fire rate to the fire rate of their respective weapon
        weaponFireRate = fireRateCyan;

        // Too many lasers to play the sound for each, play it only sometimes one is shot
        System.Random rand = new System.Random();
        int randNumber = rand.Next(1,7);

        // Check if enought time has passed since the last shot and if the player has enough ammo
        if (ReadyToShoot(weaponFireRate) && (currentCyanAmmo > 0))
        {
            //Play the Audio of the laser being shot, but dont play it too often
            if(randNumber == 3)
                ShootAudio();

            //create new object with Laser prefab from left cannon
            Instantiate(cyanBullet, cannonLeft.transform.position, transform.rotation);
            Instantiate(cyanBullet, cannonRight.transform.position, transform.rotation);

            currentCyanAmmo--;      // Reduce the amount of ammo in the cyan gun
            fireRateTimer = 0;      // Reset the timer used for the fire rate
        }

        //Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentCyanAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    private void Charge()
    {
        fireRateTimer += Time.deltaTime;    // Start a timer
    }

    private bool ReadyToShoot(float rate)
    {
        // Check if the timer exceeded the player's fire rate
        if (fireRateTimer >= rate)
            return true;
        else
            return false;
    }

    public void UseGreenWeapon()
    {
        currentWeapon = (int)Weapons.green; // Switch to the green weapon
    }

    public void UseOrangeWeapon()
    {
        currentWeapon = (int)Weapons.orange;    // Switch to the orange weapon
    }

    public void UsePinkWeapon()
    {
        currentWeapon = (int)Weapons.pink;  // Switch to the pink weapon
    }

    public void UseCyanWeapon()
    {
        currentWeapon = (int)Weapons.cyan;  // Switch to the cyan weapon
    }

    /*This is only neccessary when playing on computer
    You can press the 1,2,3,4 keybinds to change weapon
    (Both pc and mobile can also use the buttons on the GUI)*/
    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseGreenWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseOrangeWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UsePinkWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseCyanWeapon();
    }

    //Passiely heal a certain amount every x seconds
    private void PassiveHeal(float amount, float frequency)
    {
        // Start a timer
        healTimer += Time.deltaTime;

        // Check if the timer has passed the frequency while the player is still alive
        if (healTimer >= frequency && IsAlive() == true)
        {
            // Heal the player
            playerHealth = playerHealth + amount;

            // Don't heal past the player's maximum possbile health
            if (playerHealth >= maxPlayerHealth)
                playerHealth = maxPlayerHealth;

            // Reset the timer
            healTimer = 0;
        }
    }

    public bool IsAlive()
    {
        // Check is the player has more than 0 health
        if (playerHealth > 0)
            return true;
        else
            return false;
    }

    private void Die()
    {
        // Check if the player is dead
        if (IsAlive() == false)
        {
            // Save the player's score to a text file
            scoreScript.SaveScore();

            //Play the effects of the player exploding
            playerAudio.PlayOneShot(explosionSound, volume * 20f);
            explosionParticle.Play();

            // Disable the player
            gameObject.SetActive(false);

            // Disable the player's HUD on death
            topHUD.SetActive(false);
            bottomHUD.SetActive(false);
            centreHUD.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        // Check if the player collided with any ammunition or repulsors
        if (col.gameObject.tag == "Ammunition")
        {
            if (isColliding) return;
            isColliding = true;
            Destroy(col.gameObject);
            playerHealth -= 60;       // Update the player's health
        }

        
        if(col.gameObject.tag == "Repulsor")
        {
            if (isColliding) return;
            isColliding = true;
            Destroy(col.gameObject);    // Destory collider
            playerHealth -= 60;         // Update the player's health
        }

        
        // Check if the player collided with an enemy
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);    // Destroy collider
            playerHealth = 0;           // Update the player's health
        }

        // Check if the player collided with an asteroid
        if (col.gameObject.tag == "Asteroid")
        {
            Debug.Log("HELLO???");
            playerHealth = 0;   // Update the player's health
        }

        // Check if the player collided with the orange pickup
        if (col.gameObject.tag == "PickupOrange")
        {
            Destroy(col.gameObject);            // Destory collider
            currentOrangeAmmo = maxOrangeAmmo;  // Replenish respectice ammo
        }

        // Check if the player collided with the pink pickup
        if (col.gameObject.tag == "PickupPink")
        {
            Destroy(col.gameObject);        // Destory the pickup
            currentPinkAmmo = maxPinkAmmo;  // Replenish respective ammo
        }

        // Check if the player collided with the cyan pickup
        if (col.gameObject.tag == "PickupCyan")
        {
            Destroy(col.gameObject);        // Destroy the pickup
            currentCyanAmmo = maxCyanAmmo;  // Replenish respective ammo
        }
    }
}
