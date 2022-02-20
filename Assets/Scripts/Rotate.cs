using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    enum Rotations
    {
        clockwise = 0,
        counterClockwise = 1
    }

    public int rotationSpeed;
    [SerializeField] private int startingRotation;

    private void Awake()
    {
        System.Random rand = new System.Random();

        startingRotation = rand.Next(0, 2); //index is a random number between 0 and the length of the array
    }
    // Update is called once per frame
    void Update()
    {
        Rotation();   
    }

    private void Rotation()
    {
        if(startingRotation == (int)Rotations.clockwise)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        else if(startingRotation == (int)Rotations.counterClockwise)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
