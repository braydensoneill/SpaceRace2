using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private int playerSpeed;

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

    [SerializeField] private float fireRateTimer;
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

    //Other variables
    private float afterDeathTimer;
    private float healTimer;

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
        afterDeathTimer = 0;
        fireRateTimer = 0f;
        healTimer = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Player Info Variables
        playerHealth = maxPlayerHealth;
        playerSpeed = 35;

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
        FindInputValues();
        PassiveHeal(healAmount, healFrequency);
        Charge();
        Shoot();
        ChangeWeapon();
        Die();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
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
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed * verticalInput);

        if (transform.position.z < minZBoundary)    //If the player goes too far up
            transform.position = new Vector3(transform.position.x, transform.position.y, minZBoundary);

        if (transform.position.z > maxZBoundary)    //If the player goes to far down
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZBoundary);
     }

    public void Rotation()
    {
        float rotationStrength = 50.0f;

        Quaternion playerSpriteRotation = Quaternion.Euler(
            playerSprite.transform.rotation.x + (verticalInput * rotationStrength),
            playerSprite.transform.rotation.y,
            playerSprite.transform.rotation.z); ;

        playerSprite.transform.rotation = playerSpriteRotation;
    }

    private bool CheckShootKeybind()
    {
        if ((Input.GetMouseButton(0)) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            (Input.GetKey(KeyCode.Space)))
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

    private void ShootGreen()
    {
        weaponFireRate = fireRateGreen;

        if (ReadyToShoot(weaponFireRate))
        {
            Instantiate(greenBullet, cannonFront.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon
            fireRateTimer = 0;
        }
    }

    void ShootOrange()
    {
        float bulletSpread = 10f;    //Angle of shotgun bullet spread
        weaponFireRate = fireRateOrange;   //Change the player's current fire rate to the shotgun's fire rate

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

        //Check if enough time has passed since the last shot and if there is enough ammo
        if (ReadyToShoot(weaponFireRate) && (currentOrangeAmmo > 0))
        {
            //From each cannon used by this weapon, create a new object from the ammo's prefab, with the correct rotation
            Instantiate(orangeBullet, cannonLeft.transform.position, leftBulletRotation);
            Instantiate(orangeBullet, cannonFrontLeft.transform.position, frontLeftBulletRotation);
            Instantiate(orangeBullet, cannonFront.transform.position, transform.rotation);
            Instantiate(orangeBullet, cannonRight.transform.position, rightBulletRotation);
            Instantiate(orangeBullet, cannonFrontRight.transform.position, frontRightBulletRotation);

            currentOrangeAmmo--; //reduce the amount of ammo in the shotgun
            fireRateTimer = 0;    //reset the timer used for the fire rate
        }

        //Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentOrangeAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    void ShootPink()
    {
        weaponFireRate = fireRatePink;

        if (ReadyToShoot(weaponFireRate) && (currentPinkAmmo > 0))
        {
            Instantiate(pinkBullet, cannonFront.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon

            currentPinkAmmo--;
            fireRateTimer = 0;
        }

        //Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentPinkAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    void ShootCyan()
    {
        weaponFireRate = fireRateCyan;

        if (ReadyToShoot(weaponFireRate) && (currentCyanAmmo > 0))
        {
            Instantiate(cyanBullet, cannonLeft.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon
            Instantiate(cyanBullet, cannonRight.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon

            currentCyanAmmo--;
            fireRateTimer = 0;
        }

        //Change to the green weapon if you run out of this weapon's ammo while shotting it
        else if (currentCyanAmmo <= 0)
        {
            UseGreenWeapon();
        }
    }

    private void Charge()
    {
        fireRateTimer += Time.deltaTime; //start the timer
    }

    private bool ReadyToShoot(float rate)
    {
        if (fireRateTimer >= rate)
            return true;
        else
            return false;
    }

    public void UseGreenWeapon()
    {
        currentWeapon = (int)Weapons.green;
    }

    public void UseOrangeWeapon()
    {
        currentWeapon = (int)Weapons.orange;
    }

    public void UsePinkWeapon()
    {
        currentWeapon = (int)Weapons.pink;
    }

    public void UseCyanWeapon()
    {
        currentWeapon = (int)Weapons.cyan;
    }

    //This is only neccessary when testing on computer
    //The buttons for phone will direct straight to the Use functions
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

    //Passiely heal 5hp every [x] seconds
    private void PassiveHeal(float amount, float frequency)
    {
        healTimer += Time.deltaTime;

        if (healTimer >= frequency && IsAlive() == true)
        {
            playerHealth = playerHealth + amount;

            if (playerHealth >= maxPlayerHealth)
                playerHealth = maxPlayerHealth;
            healTimer = 0;
        }
    }

    private bool IsAlive()
    {
        if (playerHealth > 0)
            return true;
        else
            return false;
    }

    private void Die()
    {
        float secondsToWait = 3;

        if (IsAlive() == false)
        {
            Destroy(gameObject);    //Destroy the player
            //Charge(afterDeathTimer);    //Start a timer after the player dies

            //If the timer reaches a certain value, freeze the game and the 
            if(afterDeathTimer > secondsToWait)
                Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //If the player collides with ammunition, destroy the ammo and reduce the player's health by 1
        if (col.gameObject.tag == "Ammunition")
        {
            col.gameObject.SetActive(false);
            playerHealth -= 60;
        }

        //If the player collides with an enemy, destroy the enemy and reduce the player's health to zero
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
            playerHealth = 0;
        }

        //If the player collides with an asteroid, reduce the player's health to zero
        if (col.gameObject.tag == "Asteroid")
        {
            playerHealth = 0;
        }

        //If the player collides with the shotgun pickup, replenish the respective weapon's ammo
        if (col.gameObject.tag == "PickupOrange")
        {
            Destroy(col.gameObject);
            currentOrangeAmmo = maxOrangeAmmo;
        }

        //If the player collides with the laser pickup, replenish the respective weapon's ammo
        if (col.gameObject.tag == "PickupPink")
        {
            Destroy(col.gameObject);
            currentPinkAmmo = maxPinkAmmo;
        }

        //If the player collides with the repulsor pickup, replenish the respective weapon's ammo
        if (col.gameObject.tag == "PickupCyan")
        {
            Destroy(col.gameObject);
            currentCyanAmmo = maxCyanAmmo;
        }
    }
}
