using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // An enum containing the rotations
    enum Rotations
    {
        clockwise = 0,          
        counterClockwise = 1
    }

    // Rotation variables
    public int rotationSpeed;
    [SerializeField] private int startingRotation;

    private void Awake()
    {
        System.Random rand = new System.Random();

        //index is a random number between 0 and the length of the array
        startingRotation = rand.Next(0, 2);
    }
    // Update is called once per frame
    void Update()
    {
        Rotation();   // Rotate the gameObject
    }

    private void Rotation()
    {
        // Check if the object wants to rotate clockwise and update accordingly
        if(startingRotation == (int)Rotations.clockwise)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Check if the object wants to rotate anti clockwise and update accordingly
        else if(startingRotation == (int)Rotations.counterClockwise)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
