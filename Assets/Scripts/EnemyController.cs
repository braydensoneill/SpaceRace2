using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject cannonFront;
    public GameObject bullet;

    [SerializeField] private float speed;
    [SerializeField] private float maxRange;

    [SerializeField] private float chargeTimer;
    [SerializeField] private float weaponFireRate;
    private float fireRatePistol;


    private void Start()
    {
        speed = 12.5f;
        maxRange = -20;

        fireRatePistol = 1.5f;
    }

    private void Update()
    {
        Timer();
        ShootPistol();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);  //Shoot laser forward from player

        if (transform.position.x <= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }

    private void ShootPistol()
    {
        weaponFireRate = fireRatePistol;

        if (ReadyToSpawn(weaponFireRate))
        {
            Instantiate(bullet, cannonFront.transform.position, transform.rotation);  //create new object with Laser prefab from left cannon
            chargeTimer = 0;
        }
    }

    private void Timer()
    {
        chargeTimer += Time.deltaTime; //start the timer
    }

    private bool ReadyToSpawn(float rate)
    {
        if (chargeTimer >= rate)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
