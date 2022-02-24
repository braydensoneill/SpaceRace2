using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject cannonLeft;
    public GameObject cannonFrontLeft;
    public GameObject cannonFront;
    public GameObject cannonFrontRight;
    public GameObject cannonRight;

    public GameObject bullet;

    [SerializeField] private float speed;
    [SerializeField] private float maxRange;

    [SerializeField] private float fireRateTimer;
    [SerializeField] private float weaponFireRate;
    private float fireRate;


    private void Start()
    {
        speed = 12.5f;
        maxRange = -20;

        fireRate = 2.15f;
    }

    private void Update()
    {
        Timer();

        if (gameObject.name == "EnemyOrange(Clone)")
            ShootOrange();

        else if (gameObject.name == "EnemyPink(Clone)")
            ShootNormal();

        else
            ShootNormal();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);  //Shoot laser forward from player

        if (transform.position.x <= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }

    private void ShootNormal()
    {
        if (ReadyToShoot(fireRate))
        {
            Instantiate(bullet, cannonFront.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon
            fireRateTimer = 0;
        }
    }

    private void ShootOrange()
    {
        float bulletSpread = 2.5f;    //Angle of shotgun bullet spread

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

        if (ReadyToShoot(fireRate))
        {
            //From each cannon used by this weapon, create a new object from the ammo's prefab, with the correct rotation
            Instantiate(bullet, cannonLeft.transform.position, leftBulletRotation);
            Instantiate(bullet, cannonFrontLeft.transform.position, frontLeftBulletRotation);
            Instantiate(bullet, cannonFront.transform.position, transform.rotation);
            Instantiate(bullet, cannonRight.transform.position, rightBulletRotation);
            Instantiate(bullet, cannonFrontRight.transform.position, frontRightBulletRotation);
            fireRateTimer = 0;    //reset the timer used for the fire rate
        }
    }

    private void Timer()
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
