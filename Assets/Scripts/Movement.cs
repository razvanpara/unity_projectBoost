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
    [SerializeField] float mainThrust = 1000;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float rotationThrust = 90;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem mainThrusterParticles;
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
                aSrc.PlayOneShot(mainEngine);
        }
        else
            aSrc.Stop();
    }
    private void ProcessThrust(PlayerInput input)
    {
        if (input.Thrust)
        {
            rb.AddRelativeForce(Vector3.up.DtV() * mainThrust);
            if (!mainThrusterParticles.isPlaying)
                mainThrusterParticles.Play();
        }
        else
            mainThrusterParticles.Stop();
    }
    private void ProcessRotation(PlayerInput input)
    {
        if (input.RotateLeft)
        {
            leftThrusterParticles.Stop();
            ApplyRotation(rotationThrust);
            if (!rightThrusterParticles.isPlaying)
                rightThrusterParticles.Play();
        }
        else if (input.RotateRight)
        {
            rightThrusterParticles.Stop();
            ApplyRotation(-rotationThrust);
            if (!leftThrusterParticles.isPlaying)
                leftThrusterParticles.Play();
        }
        else
        {
            rightThrusterParticles.Stop();
            leftThrusterParticles.Stop();
        }
    }
    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward.DtV() * rotationThisFrame);
        rb.freezeRotation = false;
    }
}
