using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsorPlayerController : MonoBehaviour
{
    ScoreManager score;

    private float force;
    private float speed;
    private float maxRange;

    private void Awake()
    {
        score = FindObjectOfType<ScoreManager>();
    }

    void Start()
    {
        speed = 30;
        maxRange = 20;
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

    private void CheckOutOfBounds()
    {
        if (transform.position.x >= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }

    //This does not work Properly. I can't find/manipulate the rotation of a collider
    private void Repulse(Collider col)
    {
        Quaternion repulsedObjectRotation = Quaternion.Euler(
                col.transform.rotation.eulerAngles.x,
                col.transform.rotation.eulerAngles.y - 180,
                col.transform.rotation.eulerAngles.z);

        col.transform.rotation = repulsedObjectRotation;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ammunition")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Enemy")
        {
            //gameObject.tag = "Untagged";
            //destroyed = true;
            //obstacleSprite.GetComponent<Renderer>().enabled = false;    

            score.AddScore(1); //score is added when an obstacle is destroyed
            //explosionParticle.Play();   //play the explostion particle effect at the position of the destoyed obstacle
            //obstacleAudio.PlayOneShot(explosionSound, volume);
            Destroy(col.gameObject);    //Destroy the laser on collision
        }
    }
}
