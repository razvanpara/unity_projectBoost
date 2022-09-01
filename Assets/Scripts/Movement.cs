using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeltaTimeValuesExt
{
    public static Vector3 DtV(this Vector3 vector) => vector * Time.deltaTime;
    public static float DtV(this float n) => n * Time.deltaTime;
    public static float DtV(this double n) => (float)(n * Time.deltaTime);
    public static float DtV(this int n) => (float)n * Time.deltaTime;
}


public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 1000;
    private float rotationThrust = 90;
    private Rigidbody rb;
    private AudioSource aSrc;
    private float dt;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        var playerInput = GetPlayerInput();
        ProcessInput(playerInput);
        PlayThrustSound(playerInput);
    }

    internal class PlayerInput
    {
        public bool RotateLeft { get; set; }
        public bool RotateRight { get; set; }
        public bool Thrust { get; set; }
    }
    private PlayerInput GetPlayerInput()
    {
        return new PlayerInput
        {
            Thrust = Input.GetKey(KeyCode.Space),
            RotateLeft = Input.GetKey(KeyCode.A),
            RotateRight = Input.GetKey(KeyCode.D)
        };
    }
    private void ProcessInput(PlayerInput input)
    {
        ProcessThrust(input);
        ProcessRotation(input);
    }
    private void PlayThrustSound(PlayerInput input)
    {
        if (input.Thrust)
        {
            if (!aSrc.isPlaying)
                aSrc.Play();
        }
        else
            aSrc.Stop();
    }
    private void ProcessThrust(PlayerInput input)
    {
        if (input.Thrust)
            rb.AddRelativeForce(Vector3.up.DtV() * mainThrust);
    }
    private void ProcessRotation(PlayerInput input)
    {
        if (input.RotateLeft)
            ApplyRotation(rotationThrust);
        else if (input.RotateRight)
            ApplyRotation(-rotationThrust);
    }
    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward.DtV() * rotationThisFrame);
        rb.freezeRotation = false;
    }
}
