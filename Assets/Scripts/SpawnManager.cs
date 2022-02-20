using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    System.Random rand = new System.Random();

    public GameObject[] enemyPrefabs;

    private int minZBoundary;
    private int maxZBoundary;

    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
        spawnRate = 0.40f;

        minZBoundary = -7;
        maxZBoundary = 7;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        Spawn();
    }

    private void Spawn()
    {
        if(ReadyToSpawn(spawnRate))
        {
            Instantiate(
                enemyPrefabs[RandomIndex()], //spawn a random one of the rock prefabs
                randomPosition(),   //spawn the obstacle in a random position within the boundries
                enemyPrefabs[RandomIndex()].transform.rotation);   //set the rotation of the obstacle ///TBD
            
            spawnTimer = 0;
        }
    }

    private int RandomIndex()
    {
        int index;

        index = rand.Next(0, enemyPrefabs.Length); //index is a random number between 0 and the length of the array

        return index;
    }

    private Vector3 randomPosition()
    {
        System.Random rand = new System.Random();

        Vector3 enemyPosition;
        float xPosition = 45f;
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
        if (spawnTimer >= rate)
            return true;
        else
            return false;
    }
}
