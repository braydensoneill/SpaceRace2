using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayerController : MonoBehaviour
{
    ScoreManager score;
    private float speed;
    private float maxRange;

    void Awake()
    {
        score = FindObjectOfType<ScoreManager>();
    }

    void Start()
    {
        speed = 75;
        maxRange = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        CheckOutOfBounds();
    }
    private void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);  //Shoot laser forward from player
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.x >= maxRange)
            Destroy(gameObject);    //Destroy the laser after it exceeds a certain range
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ammunition")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }
}
