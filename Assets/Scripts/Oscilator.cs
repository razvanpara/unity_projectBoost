using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;
    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        float cycles = Time.time / period; // continually growing over time

        const float tau = Mathf.PI * 2; // constant value of 6.283
        
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset * rawSinWave;
    }
}
