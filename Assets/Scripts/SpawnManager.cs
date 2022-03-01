using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    System.Random rand = new System.Random();

    // List of all possible prefabs that can spawn
    public GameObject[] enemyPrefabs;

    // Boundary variables
    private int minZBoundary;
    private int maxZBoundary;

    // Spawn variables
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;     // Set the starting value of the timer
        spawnRate = 0.40f;  // Set how frequently objects spawn

        minZBoundary = -7;  // Set the min Z boundary
        maxZBoundary = 7;   // Set the max Z boundary
    }

    // Update is called once per frame
    void Update()
    {
        Timer();    // Keep the a timer ticking
        Spawn();    // Spawn a random object
    }

    private void Spawn()
    {
        // Check if the timer is ready spawn an object
        if(ReadyToSpawn(spawnRate))
        {
            // Instantiate an object at random from the list of prefabs
            Instantiate(
                enemyPrefabs[RandomIndex()], //spawn a random one of the rock prefabs
                randomPosition(),   //spawn the obstacle in a random position within the boundries
                enemyPrefabs[RandomIndex()].transform.rotation);   //set the rotation of the obstacle
            
            spawnTimer = 0; // Reset the timer
        }
    }

    private int RandomIndex()
    {
        int index;

        //index is a random number between 0 and the length of the array
        index = rand.Next(0, enemyPrefabs.Length);

        return index;
    }

    private Vector3 randomPosition()
    {
        System.Random rand = new System.Random();

        // Variables to hold the random object's position
        Vector3 enemyPosition;
        float xPosition = 55f;
        float yPosition = 0.5f;
        float zPosition = rand.Next(minZBoundary, maxZBoundary);

        //set the position to be a randomXYZ values within the boundries
        enemyPosition = new Vector3(
            xPosition,
            yPosition,
            zPosition);

        return enemyPosition;
    }

    private void Timer()
    {
        spawnTimer += Time.deltaTime; //start the timer
    }

    private bool ReadyToSpawn(float rate)
    {
        // Check if the timer exceeds the spawn rate
        if (spawnTimer >= rate)
            return true;
        else
            return false;
    }
}
