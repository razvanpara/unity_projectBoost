using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] float movementRate = 1F;
    [SerializeField] float maxMove = 15f;
    float initialPos = 0f;
    float td = 0f;
    bool goUp = false;
    void Start()
    {
        initialPos = transform.position.y;
    }

    void Update()
    {
        td = Time.deltaTime;
        float currentPos = transform.position.y;

        if (goUp)
        {
            transform.Translate(Vector3.up * td * movementRate);
            if (currentPos >= initialPos + maxMove)
                goUp = false;
        }
        else
        {
            transform.Translate(Vector3.down * td * movementRate);
            if (currentPos <= initialPos)
                goUp = true;
        }

    }
}
